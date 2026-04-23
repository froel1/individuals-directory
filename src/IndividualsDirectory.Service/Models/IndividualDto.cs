namespace IndividualsDirectory.Service.Models;

public record IndividualDto(
    int Id,
    string FirstName,
    string LastName,
    string PersonalNumber,
    DateOnly DateOfBirth,
    string? PhoneNumber,
    string? Email);

public record CreateIndividualRequest(
    string FirstName,
    string LastName,
    string PersonalNumber,
    DateOnly DateOfBirth,
    string? PhoneNumber,
    string? Email);

public record UpdateIndividualRequest(
    string FirstName,
    string LastName,
    string PersonalNumber,
    DateOnly DateOfBirth,
    string? PhoneNumber,
    string? Email);
