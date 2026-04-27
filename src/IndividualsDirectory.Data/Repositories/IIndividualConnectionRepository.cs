using IndividualsDirectory.Data.Entities;

namespace IndividualsDirectory.Data.Repositories;

public interface IIndividualConnectionRepository
{
    Task<IReadOnlyList<IndividualConnection>> GetByOwnerAsync(int ownerId, CancellationToken ct = default);
    Task<IReadOnlyList<IndividualConnection>> GetAllInvolvingAsync(int individualId, CancellationToken ct = default);
    Task<IReadOnlyList<IndividualConnection>> GetReferencesToAsync(int individualId, CancellationToken ct = default);
    void Add(IndividualConnection connection);
    void Remove(IndividualConnection connection);
    void RemoveRange(IEnumerable<IndividualConnection> connections);
}