using IndividualsDirectory.Data.Context;
using IndividualsDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IndividualsDirectory.Data.Repositories;

public class IndividualConnectionRepository(AppDbContext context) : IIndividualConnectionRepository
{
    public async Task<IReadOnlyList<IndividualConnection>> GetByOwnerAsync(int ownerId, CancellationToken ct = default) =>
        await context.IndividualConnections
            .Include(x => x.ConnectedIndividual)
            .Where(x => x.IndividualId == ownerId)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<IndividualConnection>> GetAllInvolvingAsync(int individualId, CancellationToken ct = default) =>
        await context.IndividualConnections
            .Where(x => x.IndividualId == individualId || x.ConnectedIndividualId == individualId)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<IndividualConnection>> GetReferencesToAsync(int individualId, CancellationToken ct = default) =>
        await context.IndividualConnections
            .Where(x => x.ConnectedIndividualId == individualId)
            .ToListAsync(ct);

    public void Add(IndividualConnection connection) => context.IndividualConnections.Add(connection);

    public void Remove(IndividualConnection connection) => context.IndividualConnections.Remove(connection);

    public void RemoveRange(IEnumerable<IndividualConnection> connections) =>
        context.IndividualConnections.RemoveRange(connections);
}