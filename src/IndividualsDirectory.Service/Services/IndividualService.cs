using IndividualsDirectory.Data.Entities;
using IndividualsDirectory.Data.UnitOfWork;
using IndividualsDirectory.Service.Exceptions;
using IndividualsDirectory.Service.Images;
using IndividualsDirectory.Service.Models;
using IndividualsDirectory.Service.Models.Shared;
using Contact = IndividualsDirectory.Service.Models.Shared.Contact;
using ContactEntity = IndividualsDirectory.Data.Entities.Contact;

namespace IndividualsDirectory.Service.Services;

public class IndividualService(
    IImageStorageService imageStorage,
    IUnitOfWork uow) : IIndividualService
{
    public async Task<int> CreateAsync(CreateIndividualRequest request, CancellationToken ct = default)
    {
        await EnsureCityExistsAsync(request.CityId, ct);
        await EnsurePersonalNumberAvailableAsync(request.PersonalNumber, null, ct);

        if (request.ConnectedIndividuals is {Count: > 0} incoming) await EnsureConnectionsValidAsync(null, incoming.Select(c => c.IndividualId), ct);

        var individual = new Individual
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Gender = request.Gender,
            PersonalNumber = request.PersonalNumber,
            DateOfBirth = request.DateOfBirth,
            CityId = request.CityId,
            Contacts = request.Contacts
                .Select(c => new ContactEntity {Type = c.Type, Number = c.Number})
                .ToList()
        };

        if (request.ConnectedIndividuals is {Count: > 0} cs)
            individual.Connections = cs
                .Select(c => new IndividualConnection
                {
                    ConnectedIndividualId = c.IndividualId,
                    ConnectionType = c.ConnectionType
                })
                .ToList();

        uow.Individuals.Add(individual);
        await uow.SaveChangesAsync(ct);
        return individual.Id;
    }

    public async Task<bool> UpdateAsync(int id, UpdateIndividualRequest request, CancellationToken ct = default)
    {
        var individual = await uow.Individuals.GetByIdWithDetailsAsync(id, ct);
        if (individual is null) return false;

        if (request.CityId.HasValue) await EnsureCityExistsAsync(request.CityId.Value, ct);
        if (request.PersonalNumber != null) await EnsurePersonalNumberAvailableAsync(request.PersonalNumber, id, ct);

        if (request.FirstName != null) individual.FirstName = request.FirstName;
        if (request.LastName != null) individual.LastName = request.LastName;
        if (request.Gender.HasValue) individual.Gender = request.Gender.Value;
        if (request.PersonalNumber != null) individual.PersonalNumber = request.PersonalNumber;
        if (request.DateOfBirth.HasValue) individual.DateOfBirth = request.DateOfBirth.Value;
        if (request.CityId.HasValue) individual.CityId = request.CityId.Value;
        if (request.ImageId.HasValue) individual.ImageId = request.ImageId;

        if (request.Contacts != null)
        {
            individual.Contacts.Clear();
            foreach (var c in request.Contacts) individual.Contacts.Add(new ContactEntity {Type = c.Type, Number = c.Number});
        }

        await uow.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var individual = await uow.Individuals.GetByIdAsync(id, ct);
        if (individual is null) return false;
        
        var inboundReferences = await uow.IndividualConnections.GetReferencesToAsync(id, ct);
        if (inboundReferences.Count > 0) uow.IndividualConnections.RemoveRange(inboundReferences);

        uow.Individuals.Remove(individual);
        await uow.SaveChangesAsync(ct);

        if (individual.ImageId.HasValue) await imageStorage.DeleteAsync(individual.ImageId.Value, ct);

        return true;
    }

    public async Task<IndividualDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await uow.Individuals.GetByIdWithDetailsAsync(id, ct);
        if (entity is null) return null;

        return new IndividualDetailsDto(
            entity.Id,
            entity.FirstName,
            entity.LastName,
            null,
            entity.Gender,
            entity.PersonalNumber,
            entity.DateOfBirth,
            entity.CityId,
            entity.ImageId,
            entity.Contacts.Select(c => new Contact(c.Number, c.Type)).ToList(),
            entity.Connections.Select(c => new ConnectedIndividualDetailsDto(
                c.ConnectedIndividualId,
                c.ConnectedIndividual.FirstName,
                c.ConnectedIndividual.LastName,
                c.ConnectionType)).ToList());
    }

    public async Task<PagedResult<IndividualListItemDto>> QuickSearchAsync(QuickSearchRequest request, CancellationToken ct = default)
    {
        var (items, total) = await uow.Individuals.QuickSearchAsync(
            request.FirstName,
            request.LastName,
            request.PersonalNumber,
            request.Page,
            request.PageSize,
            ct);

        return new PagedResult<IndividualListItemDto>(
            items.Select(MapToListItem).ToList(),
            total,
            request.Page,
            request.PageSize);
    }

    public async Task<PagedResult<IndividualListItemDto>> DetailedSearchAsync(DetailedSearchRequest request, CancellationToken ct = default)
    {
        var (items, total) = await uow.Individuals.DetailedSearchAsync(
            request.FirstName,
            request.LastName,
            request.Gender,
            request.PersonalNumber,
            request.DateOfBirth,
            request.CityId,
            request.Page,
            request.PageSize,
            ct);

        return new PagedResult<IndividualListItemDto>(
            items.Select(MapToListItem).ToList(),
            total,
            request.Page,
            request.PageSize);
    }

    public async Task<bool> UpdateConnectionsAsync(int ownerId, IReadOnlyList<ConnectedIndividual> desired, CancellationToken ct = default)
    {
        if (!await uow.Individuals.ExistsAsync(ownerId, ct)) return false;

        await EnsureConnectionsValidAsync(ownerId, desired.Select(d => d.IndividualId), ct);

        var allInvolving = await uow.IndividualConnections.GetAllInvolvingAsync(ownerId, ct);

        var existingPairs = allInvolving
            .GroupBy(c => c.IndividualId == ownerId ? c.ConnectedIndividualId : c.IndividualId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var desiredByOtherId = desired.ToDictionary(x => x.IndividualId);

        var toRemove = existingPairs
            .Where(kvp => !desiredByOtherId.ContainsKey(kvp.Key))
            .SelectMany(kvp => kvp.Value)
            .ToList();
        if (toRemove.Count > 0) uow.IndividualConnections.RemoveRange(toRemove);

        foreach (var d in desired)
            if (existingPairs.TryGetValue(d.IndividualId, out var existingRows))
            {
                foreach (var row in existingRows) row.ConnectionType = d.ConnectionType;

                var hasForward = existingRows.Any(r => r.IndividualId == ownerId);
                var hasReverse = existingRows.Any(r => r.ConnectedIndividualId == ownerId);

                if (!hasForward)
                    uow.IndividualConnections.Add(new IndividualConnection
                    {
                        IndividualId = ownerId,
                        ConnectedIndividualId = d.IndividualId,
                        ConnectionType = d.ConnectionType
                    });
                if (!hasReverse)
                    uow.IndividualConnections.Add(new IndividualConnection
                    {
                        IndividualId = d.IndividualId,
                        ConnectedIndividualId = ownerId,
                        ConnectionType = d.ConnectionType
                    });
            }
            else
            {
                uow.IndividualConnections.Add(new IndividualConnection
                {
                    IndividualId = ownerId,
                    ConnectedIndividualId = d.IndividualId,
                    ConnectionType = d.ConnectionType
                });
                uow.IndividualConnections.Add(new IndividualConnection
                {
                    IndividualId = d.IndividualId,
                    ConnectedIndividualId = ownerId,
                    ConnectionType = d.ConnectionType
                });
            }

        await uow.SaveChangesAsync(ct);
        return true;
    }

    public async Task<IReadOnlyList<ConnectionGroupDto>?> GetConnectionsGroupedAsync(int ownerId, CancellationToken ct = default)
    {
        if (!await uow.Individuals.ExistsAsync(ownerId, ct)) return null;

        var conns = await uow.IndividualConnections.GetByOwnerAsync(ownerId, ct);

        return conns
            .GroupBy(c => c.ConnectionType)
            .Select(g => new ConnectionGroupDto(
                g.Key,
                g.Count(),
                g.Select(c => new ConnectedIndividualSummaryDto(
                    c.ConnectedIndividualId,
                    c.ConnectedIndividual.FirstName,
                    c.ConnectedIndividual.LastName)).ToList()))
            .ToList();
    }

    public async Task<Guid?> UploadImageAsync(int individualId, Stream content, string fileName, CancellationToken ct = default)
    {
        var individual = await uow.Individuals.GetByIdAsync(individualId, ct);
        if (individual is null) return null;

        var newImageId = await imageStorage.SaveAsync(content, fileName, ct);
        var oldImageId = individual.ImageId;

        individual.ImageId = newImageId;
        await uow.SaveChangesAsync(ct);

        if (oldImageId.HasValue) await imageStorage.DeleteAsync(oldImageId.Value, ct);

        return newImageId;
    }

    public async Task<IReadOnlyList<IndividualConnectionCountsDto>> GetAllConnectionCountsAsync(CancellationToken ct = default)
    {
        var rows = await uow.Individuals.GetConnectionCountsAsync(ct);

        return rows
            .GroupBy(r => new {r.IndividualId, r.FirstName, r.LastName})
            .Select(g => new IndividualConnectionCountsDto(
                g.Key.IndividualId,
                g.Key.FirstName,
                g.Key.LastName,
                g.Select(r => new ConnectionTypeCount(r.ConnectionType, r.Count)).ToList()))
            .OrderBy(x => x.IndividualId)
            .ToList();
    }

    private async Task EnsureCityExistsAsync(int cityId, CancellationToken ct)
    {
        if (!await uow.Cities.ExistsAsync(cityId, ct)) throw new DomainValidationException("Validation_City_NotFound", cityId);
    }

    private async Task EnsurePersonalNumberAvailableAsync(string personalNumber, int? excludeId, CancellationToken ct)
    {
        if (await uow.Individuals.PersonalNumberExistsAsync(personalNumber, excludeId, ct)) throw new DomainValidationException("Validation_PersonalNumber_Duplicate", personalNumber);
    }

    private async Task EnsureConnectionsValidAsync(int? ownerId, IEnumerable<int> connectedIds, CancellationToken ct)
    {
        var ids = connectedIds.Distinct().ToList();
        if (ids.Count == 0) return;

        if (ownerId.HasValue && ids.Contains(ownerId.Value)) throw new DomainValidationException("Validation_SelfConnection");

        var existing = await uow.Individuals.GetExistingIdsAsync(ids, ct);
        var missing = ids.Except(existing).ToList();
        if (missing.Count > 0) throw new DomainValidationException("Validation_ConnectedIndividual_NotFound", string.Join(", ", missing));
    }

    private static IndividualListItemDto MapToListItem(Individual e) =>
        new(e.Id, e.FirstName, e.LastName, e.Gender, e.PersonalNumber, e.DateOfBirth, e.CityId, e.ImageId);
}