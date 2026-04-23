using IndividualsDirectory.Data.Entities;
using IndividualsDirectory.Data.Repositories;
using IndividualsDirectory.Service.Models;

namespace IndividualsDirectory.Service.Services;

public class IndividualService : IIndividualService
{
    private readonly IIndividualRepository _repository;

    public IndividualService(IIndividualRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<IndividualDto>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await _repository.GetAllAsync(ct);
        return items.Select(ToDto).ToList();
    }

    public async Task<IndividualDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _repository.GetByIdAsync(id, ct);
        return entity is null ? null : ToDto(entity);
    }

    public async Task<IndividualDto> CreateAsync(CreateIndividualRequest request, CancellationToken ct = default)
    {
        var entity = new Individual
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PersonalNumber = request.PersonalNumber,
            DateOfBirth = request.DateOfBirth,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
        };
        var created = await _repository.AddAsync(entity, ct);
        return ToDto(created);
    }

    public async Task<bool> UpdateAsync(int id, UpdateIndividualRequest request, CancellationToken ct = default)
    {
        var existing = await _repository.GetByIdAsync(id, ct);
        if (existing is null) return false;

        existing.FirstName = request.FirstName;
        existing.LastName = request.LastName;
        existing.PersonalNumber = request.PersonalNumber;
        existing.DateOfBirth = request.DateOfBirth;
        existing.PhoneNumber = request.PhoneNumber;
        existing.Email = request.Email;

        return await _repository.UpdateAsync(existing, ct);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        => _repository.DeleteAsync(id, ct);

    private static IndividualDto ToDto(Individual e) => new(
        e.Id, e.FirstName, e.LastName, e.PersonalNumber, e.DateOfBirth, e.PhoneNumber, e.Email);
}
