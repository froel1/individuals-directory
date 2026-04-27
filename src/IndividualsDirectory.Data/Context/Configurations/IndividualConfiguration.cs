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
    }
}
