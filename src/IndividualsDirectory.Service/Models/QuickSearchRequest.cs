namespace IndividualsDirectory.Service.Models;

public record QuickSearchRequest(
    string? FirstName = null,
    string? LastName = null,
    string? PersonalNumber = null,
    int Page = 1,
    int PageSize = 20);
