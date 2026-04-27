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

        builder.HasIndex(x => new { x.IndividualId, x.ConnectedIndividualId })
            .IsUnique();

        builder.HasData(
            // 1 <-> 2 (Colleague)
            new IndividualConnection { Id = 1, IndividualId = 1, ConnectedIndividualId = 2, ConnectionType = ConnectionType.Colleague },
            new IndividualConnection { Id = 2, IndividualId = 2, ConnectedIndividualId = 1, ConnectionType = ConnectionType.Colleague },
            // 1 <-> 3 (Relative)
            new IndividualConnection { Id = 3, IndividualId = 1, ConnectedIndividualId = 3, ConnectionType = ConnectionType.Relative },
            new IndividualConnection { Id = 4, IndividualId = 3, ConnectedIndividualId = 1, ConnectionType = ConnectionType.Relative },
            // 2 <-> 4 (Acquaintance)
            new IndividualConnection { Id = 5, IndividualId = 2, ConnectedIndividualId = 4, ConnectionType = ConnectionType.Acquaintance },
            new IndividualConnection { Id = 6, IndividualId = 4, ConnectedIndividualId = 2, ConnectionType = ConnectionType.Acquaintance },
            // 3 <-> 5 (Other)
            new IndividualConnection { Id = 7, IndividualId = 3, ConnectedIndividualId = 5, ConnectionType = ConnectionType.Other },
            new IndividualConnection { Id = 8, IndividualId = 5, ConnectedIndividualId = 3, ConnectionType = ConnectionType.Other });
    }
}
