using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.AdditionalContent;

namespace Streetcode.DAL.Configurations;

public class TagMap : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder
            .HasMany(t => t.Streetcodes)
            .WithMany(s => s.Tags)
            .UsingEntity<StreetcodeTagIndex>(
                sp => sp.HasOne(x => x.Streetcode)
                                                                    .WithMany(x => x.StreetcodeTagIndices)
                                                                    .HasForeignKey(x => x.StreetcodeId),
                sp => sp.HasOne(x => x.Tag)
                                                                    .WithMany(x => x.StreetcodeTagIndices)
                                                                    .HasForeignKey(x => x.TagId));
    }
}