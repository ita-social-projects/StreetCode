using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.Media.Images;

namespace Streetcode.DAL.Configurations;

public class ArtMap : IEntityTypeConfiguration<Art>
{
    public void Configure(EntityTypeBuilder<Art> builder)
    {
        builder
            .HasOne(d => d.Streetcode)
            .WithMany(d => d.Arts)
            .HasForeignKey(d => d.StreetcodeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(d => d.Image)
            .WithOne(d => d.Art)
            .HasForeignKey<Art>(d => d.ImageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}