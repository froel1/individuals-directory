using IndividualsDirectory.Data.Entities;

namespace IndividualsDirectory.Service.Models;

public record ConnectionGroupDto(
    ConnectionType ConnectionType,
    int Count,
    IReadOnlyList<ConnectedIndividualSummaryDto> Individuals);

public record ConnectedIndividualSummaryDto(
    int IndividualId,
    string FirstName,
    string LastName);
