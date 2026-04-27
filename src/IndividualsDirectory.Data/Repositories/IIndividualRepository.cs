using IndividualsDirectory.Data.Entities;

namespace IndividualsDirectory.Data.Repositories;

public interface IIndividualRepository
{
    Task<Individual?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Individual?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    Task<bool> PersonalNumberExistsAsync(string personalNumber, int? excludeId = null, CancellationToken ct = default);

    void Add(Individual individual);
    void Remove(Individual individual);

    Task<(IReadOnlyList<Individual> Items, int Total)> QuickSearchAsync(
        string? firstName,
        string? lastName,
        string? personalNumber,
        int page,
        int pageSize,
        CancellationToken ct = default);

    Task<(IReadOnlyList<Individual> Items, int Total)> DetailedSearchAsync(
        string? firstName,
        string? lastName,
        Gender? gender,
        string? personalNumber,
        DateOnly? dateOfBirth,
        int? cityId,
        int page,
        int pageSize,
        CancellationToken ct = default);
}
