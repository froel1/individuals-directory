using IndividualsDirectory.Data.Entities;

namespace IndividualsDirectory.Service.Models;

public record IndividualConnectionCountsDto(
    int IndividualId,
    string FirstName,
    string LastName,
    IReadOnlyList<ConnectionTypeCount> Counts);

public record ConnectionTypeCount(ConnectionType ConnectionType, int Count);