using IndividualsDirectory.Service.Models;
using IndividualsDirectory.Service.Models.Shared;

namespace IndividualsDirectory.Service.Services;

public interface IIndividualService
{
    Task<int> CreateAsync(CreateIndividualRequest request, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, UpdateIndividualRequest request, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    Task<IndividualDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PagedResult<IndividualListItemDto>> QuickSearchAsync(QuickSearchRequest request, CancellationToken ct = default);
    Task<PagedResult<IndividualListItemDto>> DetailedSearchAsync(DetailedSearchRequest request, CancellationToken ct = default);
    Task<bool> UpdateConnectionsAsync(int ownerId, IReadOnlyList<ConnectedIndividual> connections, CancellationToken ct = default);
    Task<IReadOnlyList<ConnectionGroupDto>?> GetConnectionsGroupedAsync(int ownerId, CancellationToken ct = default);
    Task<IReadOnlyList<IndividualConnectionCountsDto>> GetAllConnectionCountsAsync(CancellationToken ct = default);
    Task<Guid?> UploadImageAsync(int individualId, Stream content, string fileName, CancellationToken ct = default);
}
