using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.Streetcode.TextContent;

namespace Streetcode.DAL.Configurations;

public class RelatedTermMap : IEntityTypeConfiguration<RelatedTerm>
{
    public void Configure(EntityTypeBuilder<RelatedTerm> builder)
    {
        builder
            .HasOne(rt => rt.Term)
            .WithMany(t => t.RelatedTerms)
            .HasForeignKey(rt => rt.TermId);
    }
}