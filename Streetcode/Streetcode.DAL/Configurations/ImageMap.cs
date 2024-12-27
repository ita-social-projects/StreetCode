using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Entities.Partners;

namespace Streetcode.DAL.Configurations;

public class ImageMap : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasOne(d => d.Art)
            .WithOne(a => a.Image)
            .HasForeignKey<Art>(a => a.ImageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(im => im.ImageDetails)
            .WithOne(info => info.Image)
            .HasForeignKey<ImageDetails>(a => a.ImageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.Partner)
            .WithOne(p => p.Logo)
            .HasForeignKey<Partner>(d => d.LogoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Facts)
            .WithOne(p => p.Image)
            .HasForeignKey(d => d.ImageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(i => i.SourceLinkCategories)
            .WithOne(s => s.Image)
            .HasForeignKey(d => d.ImageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.News)
            .WithOne(p => p.Image)
            .HasForeignKey<News>(d => d.ImageId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}
