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

        builder.HasData(
            new Contact { Id = 1, IndividualId = 1, Type = PhoneType.Mobile, Number = "+995555111001" },
            new Contact { Id = 2, IndividualId = 1, Type = PhoneType.Office, Number = "+995322111002" },
            new Contact { Id = 3, IndividualId = 2, Type = PhoneType.Mobile, Number = "+995555222001" },
            new Contact { Id = 4, IndividualId = 3, Type = PhoneType.Mobile, Number = "+995555333001" },
            new Contact { Id = 5, IndividualId = 4, Type = PhoneType.Home, Number = "+995431444001" },
            new Contact { Id = 6, IndividualId = 5, Type = PhoneType.Mobile, Number = "+995555555001" });
    }
}
