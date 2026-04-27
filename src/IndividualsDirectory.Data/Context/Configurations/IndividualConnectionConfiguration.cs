using IndividualsDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IndividualsDirectory.Data.Context.Configurations;

public class IndividualConnectionConfiguration : IEntityTypeConfiguration<IndividualConnection>
{
    public void Configure(EntityTypeBuilder<IndividualConnection> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ConnectionType).HasConversion<int>();

        builder.HasOne(x => x.ConnectedIndividual)
            .WithMany()
            .HasForeignKey(x => x.ConnectedIndividualId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new {x.IndividualId, x.ConnectedIndividualId})
            .IsUnique();
    }
}