using IndividualsDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IndividualsDirectory.Data.Context.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Number).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Type).HasConversion<int>();
    }
}
