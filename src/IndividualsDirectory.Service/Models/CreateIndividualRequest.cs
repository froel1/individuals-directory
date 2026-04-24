using System.Text.Json.Serialization;
using IndividualsDirectory.Data.Entities;

namespace IndividualsDirectory.Service.Models;

public record CreateIndividualRequest(
    string FirstName,
    string LastName,
    Gender Gender,
    string PersonalNumber,
    DateOnly DateOfBirth,
    int CityId,
    [property: JsonIgnore] Guid? ImageId,
    List<Contact> Contacts,
    List<ConnectedIndividual>? ConnectedIndividuals);

