using IndividualsDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IndividualsDirectory.Data.Context.Configurations;

public class IndividualConfiguration : IEntityTypeConfiguration<Individual>
{
    public void Configure(EntityTypeBuilder<Individual> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.PersonalNumber).IsRequired().HasMaxLength(11);
        builder.Property(x => x.Gender).HasConversion<int>();

        builder.HasIndex(x => x.PersonalNumber).IsUnique();
        builder.HasIndex(x => x.FirstName);
        builder.HasIndex(x => x.LastName);

        builder.HasOne(x => x.City)
            .WithMany()
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Contacts)
            .WithOne(c => c.Individual)
            .HasForeignKey(c => c.IndividualId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Connections)
            .WithOne(c => c.Individual)
            .HasForeignKey(c => c.IndividualId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            new Individual { Id = 1, FirstName = "Giorgi", LastName = "Beridze", Gender = Gender.Male, PersonalNumber = "01001011001", DateOfBirth = new DateOnly(1990, 5, 15), CityId = 1 },
            new Individual { Id = 2, FirstName = "Nino", LastName = "Kapanadze", Gender = Gender.Female, PersonalNumber = "01001011002", DateOfBirth = new DateOnly(1992, 8, 20), CityId = 1 },
            new Individual { Id = 3, FirstName = "Davit", LastName = "Tsiklauri", Gender = Gender.Male, PersonalNumber = "01001011003", DateOfBirth = new DateOnly(1985, 3, 10), CityId = 2 },
            new Individual { Id = 4, FirstName = "Mariam", LastName = "Lomidze", Gender = Gender.Female, PersonalNumber = "01001011004", DateOfBirth = new DateOnly(1995, 12, 5), CityId = 3 },
            new Individual { Id = 5, FirstName = "Levan", LastName = "Adamia", Gender = Gender.Male, PersonalNumber = "01001011005", DateOfBirth = new DateOnly(1988, 7, 25), CityId = 4 });
    }
}
