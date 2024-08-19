using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Configurations;

public class StreetcodeArtSlideMap : IEntityTypeConfiguration<StreetcodeArtSlide>
{
    public void Configure(EntityTypeBuilder<StreetcodeArtSlide> builder)
    {
        builder
            .HasOne(d => d.Streetcode)
            .WithMany(d => d.StreetcodeArtSlides)
            .HasForeignKey(d => d.StreetcodeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(e => e.Index)
            .HasDefaultValue(1);
    }
}