using IndividualsDirectory.Data.Context;
using IndividualsDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IndividualsDirectory.Data.Repositories;

public class CityRepository(AppDbContext context) : ICityRepository
{
    public Task<bool> ExistsAsync(int id, CancellationToken ct = default) =>
        context.Cities.AnyAsync(x => x.Id == id, ct);

    public async Task<IReadOnlyList<City>> GetAllAsync(CancellationToken ct = default) =>
        await context.Cities.OrderBy(x => x.Name).ToListAsync(ct);
}
