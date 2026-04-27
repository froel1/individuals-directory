namespace IndividualsDirectory.Data.Entities;

public class Contact
{
    public int Id { get; set; }
    public int IndividualId { get; set; }
    public PhoneType Type { get; set; }
    public string Number { get; set; } = string.Empty;

    public Individual Individual { get; set; } = null!;
}