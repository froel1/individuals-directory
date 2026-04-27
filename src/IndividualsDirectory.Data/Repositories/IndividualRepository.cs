using IndividualsDirectory.Data.Context;
using IndividualsDirectory.Data.Entities;
using IndividualsDirectory.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace IndividualsDirectory.Data.Repositories;

public class IndividualRepository(AppDbContext context) : IIndividualRepository
{
    public Task<Individual?> GetByIdAsync(int id, CancellationToken ct = default) =>
        context.Individuals.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<Individual?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default) =>
        context.Individuals
            .Include(x => x.City)
            .Include(x => x.Contacts)
            .Include(x => x.Connections).ThenInclude(c => c.ConnectedIndividual)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<bool> ExistsAsync(int id, CancellationToken ct = default) =>
        context.Individuals.AnyAsync(x => x.Id == id, ct);

    public async Task<IReadOnlyList<int>> GetExistingIdsAsync(IEnumerable<int> ids, CancellationToken ct = default)
    {
        var requested = ids.Distinct().ToList();
        return await context.Individuals
            .Where(x => requested.Contains(x.Id))
            .Select(x => x.Id)
            .ToListAsync(ct);
    }

    public Task<bool> PersonalNumberExistsAsync(string personalNumber, int? excludeId = null, CancellationToken ct = default) =>
        context.Individuals.AnyAsync(
            x => x.PersonalNumber == personalNumber && (excludeId == null || x.Id != excludeId.Value),
            ct);

    public async Task<IReadOnlyList<ConnectionCountRow>> GetConnectionCountsAsync(CancellationToken ct = default) =>
        await context.IndividualConnections
            .GroupBy(c => new {c.IndividualId, c.Individual.FirstName, c.Individual.LastName, c.ConnectionType})
            .Select(g => new ConnectionCountRow(
                g.Key.IndividualId,
                g.Key.FirstName,
                g.Key.LastName,
                g.Key.ConnectionType,
                g.Count()))
            .ToListAsync(ct);

    public void Add(Individual individual) => context.Individuals.Add(individual);

    public void Remove(Individual individual) => context.Individuals.Remove(individual);

    public async Task<(IReadOnlyList<Individual> Items, int Total)> QuickSearchAsync(
        string? firstName,
        string? lastName,
        string? personalNumber,
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        var query = context.Individuals.AsQueryable();

        var hasFirstName = !string.IsNullOrWhiteSpace(firstName);
        var hasLastName = !string.IsNullOrWhiteSpace(lastName);
        var hasPersonalNumber = !string.IsNullOrWhiteSpace(personalNumber);

        if (hasFirstName || hasLastName || hasPersonalNumber)
        {
            var fn = $"%{firstName}%";
            var ln = $"%{lastName}%";
            var pn = $"%{personalNumber}%";

            query = query.Where(x =>
                (hasFirstName && EF.Functions.Like(x.FirstName, fn))
                || (hasLastName && EF.Functions.Like(x.LastName, ln))
                || (hasPersonalNumber && EF.Functions.Like(x.PersonalNumber, pn)));
        }

        return await PageAsync(query, page, pageSize, ct);
    }

    public async Task<(IReadOnlyList<Individual> Items, int Total)> DetailedSearchAsync(
        string? firstName,
        string? lastName,
        Gender? gender,
        string? personalNumber,
        DateOnly? dateOfBirth,
        int? cityId,
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        var query = context.Individuals.AsQueryable();

        if (!string.IsNullOrWhiteSpace(firstName))
            query = query.Where(x => x.FirstName == firstName);
        if (!string.IsNullOrWhiteSpace(lastName))
            query = query.Where(x => x.LastName == lastName);
        if (gender.HasValue)
            query = query.Where(x => x.Gender == gender.Value);
        if (!string.IsNullOrWhiteSpace(personalNumber))
            query = query.Where(x => x.PersonalNumber == personalNumber);
        if (dateOfBirth.HasValue)
            query = query.Where(x => x.DateOfBirth == dateOfBirth.Value);
        if (cityId.HasValue)
            query = query.Where(x => x.CityId == cityId.Value);

        return await PageAsync(query, page, pageSize, ct);
    }

    private static async Task<(IReadOnlyList<Individual> Items, int Total)> PageAsync(
        IQueryable<Individual> query,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(x => x.LastName).ThenBy(x => x.FirstName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
        return (items, total);
    }
}