using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.Event;

namespace Streetcode.DAL.Configurations
{
    public class EventStreetcodesMap : IEntityTypeConfiguration<EventStreetcodes>
    {
        public void Configure(EntityTypeBuilder<EventStreetcodes> builder)
        {
            builder
            .HasKey(es => new { es.EventId, es.StreetcodeId });

            builder
                .HasOne(es => es.Event)
                .WithMany(x => x.EventStreetcodes)
                .HasForeignKey(x => x.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(es => es.StreetcodeContent)
                .WithMany(x => x.EventStreetcodes)
                .HasForeignKey(x => x.StreetcodeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
