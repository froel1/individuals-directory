using IndividualsDirectory.Data.Entities;

namespace IndividualsDirectory.Service.Models.Shared;

public record ConnectedIndividual(int IndividualId, ConnectionType ConnectionType);