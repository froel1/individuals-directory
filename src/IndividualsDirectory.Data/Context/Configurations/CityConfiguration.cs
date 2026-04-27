using IndividualsDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IndividualsDirectory.Data.Context.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(
            new City {Id = 1, Name = "Tbilisi"},
            new City {Id = 2, Name = "Batumi"},
            new City {Id = 3, Name = "Kutaisi"},
            new City {Id = 4, Name = "Rustavi"},
            new City {Id = 5, Name = "Gori"},
            new City {Id = 6, Name = "Zugdidi"},
            new City {Id = 7, Name = "Poti"},
            new City {Id = 8, Name = "Telavi"});
    }
}