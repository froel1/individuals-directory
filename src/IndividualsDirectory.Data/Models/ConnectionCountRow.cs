using IndividualsDirectory.Data.Entities;

namespace IndividualsDirectory.Data.Models;

public record ConnectionCountRow(
    int IndividualId,
    string FirstName,
    string LastName,
    ConnectionType ConnectionType,
    int Count);