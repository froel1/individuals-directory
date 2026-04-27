namespace IndividualsDirectory.Data.Entities;

public class Individual
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public string PersonalNumber { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public int CityId { get; set; }
    public Guid? ImageId { get; set; }

    public City City { get; set; } = null!;
    public ICollection<Contact> Contacts { get; set; } = [];
    public ICollection<IndividualConnection> Connections { get; set; } = [];
}