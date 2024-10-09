using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.AdditionalContent;

namespace Streetcode.DAL.Configurations;

public class StreetcodeTagIndexMap : IEntityTypeConfiguration<StreetcodeTagIndex>
{
    public void Configure(EntityTypeBuilder<StreetcodeTagIndex> builder)
    {
        builder
            .HasKey(nameof(StreetcodeTagIndex.StreetcodeId), nameof(StreetcodeTagIndex.TagId));
    }
}