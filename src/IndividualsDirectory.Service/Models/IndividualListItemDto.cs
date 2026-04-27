using IndividualsDirectory.Data.Entities;

namespace IndividualsDirectory.Service.Models;

public record IndividualListItemDto(
    int Id,
    string FirstName,
    string LastName,
    Gender Gender,
    string PersonalNumber,
    DateOnly DateOfBirth,
    int CityId,
    Guid? ImageId);