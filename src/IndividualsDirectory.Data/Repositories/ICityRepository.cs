using IndividualsDirectory.Data.Entities;

namespace IndividualsDirectory.Data.Repositories;

public interface ICityRepository
{
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<City>> GetAllAsync(CancellationToken ct = default);
}