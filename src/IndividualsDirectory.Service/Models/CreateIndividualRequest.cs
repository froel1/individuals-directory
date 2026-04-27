using IndividualsDirectory.Data.Entities;
using IndividualsDirectory.Service.Models.Shared;
using Contact = IndividualsDirectory.Service.Models.Shared.Contact;

namespace IndividualsDirectory.Service.Models;

public record CreateIndividualRequest(
    string FirstName,
    string LastName,
    Gender Gender,
    string PersonalNumber,
    DateOnly DateOfBirth,
    int CityId,
    List<Contact> Contacts,
    List<ConnectedIndividual>? ConnectedIndividuals);