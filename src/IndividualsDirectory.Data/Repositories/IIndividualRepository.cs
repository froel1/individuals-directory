using IndividualsDirectory.Data.Entities;

namespace IndividualsDirectory.Data.Repositories;

public interface IIndividualRepository
{
    Task<IReadOnlyList<Individual>> GetAllAsync(CancellationToken ct = default);
    Task<Individual?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Individual> AddAsync(Individual individual, CancellationToken ct = default);
    Task<bool> UpdateAsync(Individual individual, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
