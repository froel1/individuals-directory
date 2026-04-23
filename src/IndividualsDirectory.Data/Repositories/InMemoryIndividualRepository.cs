using System.Collections.Concurrent;
using IndividualsDirectory.Data.Entities;

namespace IndividualsDirectory.Data.Repositories;

public class InMemoryIndividualRepository : IIndividualRepository
{
    private readonly ConcurrentDictionary<int, Individual> _store = new();
    private int _nextId;

    public Task<IReadOnlyList<Individual>> GetAllAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<Individual>>(_store.Values.OrderBy(x => x.Id).ToList());

    public Task<Individual?> GetByIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult(_store.TryGetValue(id, out var value) ? value : null);

    public Task<Individual> AddAsync(Individual individual, CancellationToken ct = default)
    {
        individual.Id = Interlocked.Increment(ref _nextId);
        _store[individual.Id] = individual;
        return Task.FromResult(individual);
    }

    public Task<bool> UpdateAsync(Individual individual, CancellationToken ct = default)
    {
        if (!_store.ContainsKey(individual.Id)) return Task.FromResult(false);
        _store[individual.Id] = individual;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        => Task.FromResult(_store.TryRemove(id, out _));
}
