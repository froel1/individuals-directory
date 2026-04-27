using IndividualsDirectory.Data.Entities;
using Contact = IndividualsDirectory.Service.Models.Shared.Contact;

namespace IndividualsDirectory.Service.Models;

public record IndividualDetailsDto(
    int Id,
    string FirstName,
    string LastName,
    string? ImageUrl,
    Gender Gender,
    string PersonalNumber,
    DateOnly DateOfBirth,
    int CityId,
    Guid? ImageId,
    IReadOnlyList<Contact> Contacts,
    IReadOnlyList<ConnectedIndividualDetailsDto> Connections);

public record ConnectedIndividualDetailsDto(
    int IndividualId,
    string FirstName,
    string LastName,
    ConnectionType ConnectionType);