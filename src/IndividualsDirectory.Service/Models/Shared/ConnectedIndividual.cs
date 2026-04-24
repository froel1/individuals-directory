using IndividualsDirectory.Data.Entities;

namespace IndividualsDirectory.Service.Models;

public record ConnectedIndividual(int IndividualId, ConnectionType ConnectionType);