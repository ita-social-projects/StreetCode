using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.Sources;

namespace Streetcode.DAL.Configurations;

public class SourceLinkCategoryMap : IEntityTypeConfiguration<SourceLinkCategory>
{
    public void Configure(EntityTypeBuilder<SourceLinkCategory> builder)
    {
        builder
            .HasMany(d => d.StreetcodeCategoryContents)
            .WithOne(p => p.SourceLinkCategory)
            .HasForeignKey(d => d.SourceLinkCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}