using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.Streetcode.TextContent;

namespace Streetcode.DAL.Configurations;

public class FactMap : IEntityTypeConfiguration<Fact>
{
    public void Configure(EntityTypeBuilder<Fact> builder)
    {
        builder
            .Property(b => b.Index)
            .HasDefaultValue(0);
    }
}