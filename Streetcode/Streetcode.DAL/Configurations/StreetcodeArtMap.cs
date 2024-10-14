using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Configurations;

public class StreetcodeArtMap : IEntityTypeConfiguration<StreetcodeArt>
{
    public void Configure(EntityTypeBuilder<StreetcodeArt> builder)
    {
        builder
                .HasKey(d => new { d.Id });

        builder
                .HasOne(d => d.StreetcodeArtSlide)
                .WithMany(d => d.StreetcodeArts)
                .HasForeignKey(d => d.StreetcodeArtSlideId)
                .OnDelete(DeleteBehavior.Cascade);

        builder
                .HasOne(d => d.Art)
                .WithMany(d => d.StreetcodeArts)
                .HasForeignKey(d => d.ArtId)
                .OnDelete(DeleteBehavior.Cascade);

        builder
                .Property(e => e.Index)
                .HasDefaultValue(1);

        builder
                .HasIndex(d => new { d.ArtId, d.StreetcodeArtSlideId })
                .IsUnique(false);
    }
}