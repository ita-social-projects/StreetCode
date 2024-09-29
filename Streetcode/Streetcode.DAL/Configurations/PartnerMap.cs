using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.Partners;

namespace Streetcode.DAL.Configurations;

public class PartnerMap : IEntityTypeConfiguration<Partner>
{
    public void Configure(EntityTypeBuilder<Partner> builder)
    {
        builder
            .HasMany(d => d.PartnerSourceLinks)
            .WithOne(p => p.Partner)
            .HasForeignKey(d => d.PartnerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(p => p.IsKeyPartner)
            .HasDefaultValue("false");
    }
}