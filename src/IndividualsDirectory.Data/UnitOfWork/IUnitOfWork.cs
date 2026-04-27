using IndividualsDirectory.Data.Repositories;

namespace IndividualsDirectory.Data.UnitOfWork;

public interface IUnitOfWork
{
    IIndividualRepository Individuals { get; }
    ICityRepository Cities { get; }
    IIndividualConnectionRepository IndividualConnections { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}