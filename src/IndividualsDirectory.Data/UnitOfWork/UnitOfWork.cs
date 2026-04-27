using IndividualsDirectory.Data.Context;
using IndividualsDirectory.Data.Repositories;

namespace IndividualsDirectory.Data.UnitOfWork;

public class UnitOfWork(
    AppDbContext context,
    IIndividualRepository individuals,
    ICityRepository cities,
    IIndividualConnectionRepository individualConnections) : IUnitOfWork
{
    public IIndividualRepository Individuals { get; } = individuals;
    public ICityRepository Cities { get; } = cities;
    public IIndividualConnectionRepository IndividualConnections { get; } = individualConnections;

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        context.SaveChangesAsync(ct);
}
