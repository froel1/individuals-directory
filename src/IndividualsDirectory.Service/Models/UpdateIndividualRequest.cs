using System.Text.Json.Serialization;
using IndividualsDirectory.Data.Entities;

namespace IndividualsDirectory.Service.Models;

public record UpdateIndividualRequest(
    string? FirstName,
    string? LastName,
    Gender? Gender,
    string? PersonalNumber,
    DateOnly? DateOfBirth,
    int? CityId,
    List<Contact>? Contacts,
    [property: JsonIgnore] Guid? ImageId);
