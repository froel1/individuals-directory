using IndividualsDirectory.Service.Models;

namespace IndividualsDirectory.Service.Services;

public interface IIndividualService
{
    Task<IReadOnlyList<IndividualDto>> GetAllAsync(CancellationToken ct = default);
    Task<IndividualDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IndividualDto> CreateAsync(CreateIndividualRequest request, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, UpdateIndividualRequest request, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
