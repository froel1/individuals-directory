using IndividualsDirectory.Data.Entities;

namespace IndividualsDirectory.Service.Models;

public record DetailedSearchRequest(
    string? FirstName = null,
    string? LastName = null,
    Gender? Gender = null,
    string? PersonalNumber = null,
    DateOnly? DateOfBirth = null,
    int? CityId = null,
    int Page = 1,
    int PageSize = 20);
