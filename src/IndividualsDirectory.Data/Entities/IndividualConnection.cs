namespace IndividualsDirectory.Data.Entities;

public class IndividualConnection
{
    public int Id { get; set; }
    public int IndividualId { get; set; }
    public int ConnectedIndividualId { get; set; }
    public ConnectionType ConnectionType { get; set; }

    public Individual Individual { get; set; } = null!;
    public Individual ConnectedIndividual { get; set; } = null!;
}
