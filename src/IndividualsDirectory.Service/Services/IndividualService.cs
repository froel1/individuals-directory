using IndividualsDirectory.Data.Entities;
using IndividualsDirectory.Data.Repositories;
using IndividualsDirectory.Data.UnitOfWork;
using IndividualsDirectory.Service.Images;
using IndividualsDirectory.Service.Models;
using ContactEntity = IndividualsDirectory.Data.Entities.Contact;

namespace IndividualsDirectory.Service.Services;

public class IndividualService(
    IIndividualRepository individuals,
    IIndividualConnectionRepository connections,
    IImageStorageService imageStorage,
    IUnitOfWork uow) : IIndividualService
{
    public async Task<int> CreateAsync(CreateIndividualRequest request, CancellationToken ct = default)
    {
        var individual = new Individual
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Gender = request.Gender,
            PersonalNumber = request.PersonalNumber,
            DateOfBirth = request.DateOfBirth,
            CityId = request.CityId,
            ImageId = request.ImageId,
            Contacts = request.Contacts
                .Select(c => new ContactEntity { Type = c.Type, Number = c.Number })
                .ToList(),
        };

        if (request.ConnectedIndividuals is { Count: > 0 } cs)
        {
            individual.Connections = cs
                .Select(c => new IndividualConnection
                {
                    ConnectedIndividualId = c.IndividualId,
                    ConnectionType = c.ConnectionType,
                })
                .ToList();
        }

        individuals.Add(individual);
        await uow.SaveChangesAsync(ct);
        return individual.Id;
    }

    public async Task<bool> UpdateAsync(int id, UpdateIndividualRequest request, CancellationToken ct = default)
    {
        var individual = await individuals.GetByIdWithDetailsAsync(id, ct);
        if (individual is null) return false;

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
            foreach (var c in request.Contacts)
            {
                individual.Contacts.Add(new ContactEntity { Type = c.Type, Number = c.Number });
            }
        }

        await uow.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var individual = await individuals.GetByIdAsync(id, ct);
        if (individual is null) return false;

        var inboundReferences = await connections.GetReferencesToAsync(id, ct);
        if (inboundReferences.Count > 0)
        {
            connections.RemoveRange(inboundReferences);
        }

        individuals.Remove(individual);
        await uow.SaveChangesAsync(ct);

        if (individual.ImageId.HasValue)
        {
            await imageStorage.DeleteAsync(individual.ImageId.Value, ct);
        }

        return true;
    }

    public async Task<IndividualDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await individuals.GetByIdWithDetailsAsync(id, ct);
        if (entity is null) return null;

        return new IndividualDetailsDto(
            entity.Id,
            entity.FirstName,
            entity.LastName,
            entity.Gender,
            entity.PersonalNumber,
            entity.DateOfBirth,
            entity.CityId,
            entity.ImageId,
            entity.Contacts.Select(c => new Models.Contact(c.Number, c.Type)).ToList(),
            entity.Connections.Select(c => new ConnectedIndividualDetailsDto(
                c.ConnectedIndividualId,
                c.ConnectedIndividual.FirstName,
                c.ConnectedIndividual.LastName,
                c.ConnectionType)).ToList());
    }

    public async Task<PagedResult<IndividualListItemDto>> QuickSearchAsync(QuickSearchRequest request, CancellationToken ct = default)
    {
        var (items, total) = await individuals.QuickSearchAsync(
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
        var (items, total) = await individuals.DetailedSearchAsync(
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
        if (!await individuals.ExistsAsync(ownerId, ct))
        {
            return false;
        }

        var existing = await connections.GetByOwnerAsync(ownerId, ct);
        var existingByConnectedId = existing.ToDictionary(x => x.ConnectedIndividualId);
        var desiredByConnectedId = desired.ToDictionary(x => x.IndividualId);

        var toRemove = existing
            .Where(x => !desiredByConnectedId.ContainsKey(x.ConnectedIndividualId))
            .ToList();
        if (toRemove.Count > 0)
        {
            connections.RemoveRange(toRemove);
        }

        foreach (var d in desired)
        {
            if (existingByConnectedId.TryGetValue(d.IndividualId, out var existingConn))
            {
                existingConn.ConnectionType = d.ConnectionType;
            }
            else
            {
                connections.Add(new IndividualConnection
                {
                    IndividualId = ownerId,
                    ConnectedIndividualId = d.IndividualId,
                    ConnectionType = d.ConnectionType,
                });
            }
        }

        await uow.SaveChangesAsync(ct);
        return true;
    }

    public async Task<IReadOnlyList<ConnectionGroupDto>?> GetConnectionsGroupedAsync(int ownerId, CancellationToken ct = default)
    {
        if (!await individuals.ExistsAsync(ownerId, ct))
        {
            return null;
        }

        var conns = await connections.GetByOwnerAsync(ownerId, ct);

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
        var individual = await individuals.GetByIdAsync(individualId, ct);
        if (individual is null) return null;

        var newImageId = await imageStorage.SaveAsync(content, fileName, ct);
        var oldImageId = individual.ImageId;

        individual.ImageId = newImageId;
        await uow.SaveChangesAsync(ct);

        if (oldImageId.HasValue)
        {
            await imageStorage.DeleteAsync(oldImageId.Value, ct);
        }

        return newImageId;
    }

    private static IndividualListItemDto MapToListItem(Individual e) =>
        new(e.Id, e.FirstName, e.LastName, e.Gender, e.PersonalNumber, e.DateOfBirth, e.CityId, e.ImageId);
}
