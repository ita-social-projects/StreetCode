using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.Event;
using Streetcode.DAL.Entities.Streetcode.TextContent;

namespace Streetcode.DAL.Configurations
{
    public class EventMap : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder
               .HasDiscriminator<string>("EventType")
               .HasValue<HistoricalEvent>("Historical")
               .HasValue<CustomEvent>("Custom");

            builder
                .HasMany(e => e.Streetcodes)
                .WithMany(sc => sc.Events)
                .UsingEntity(j => j.ToTable("EventStreetcodes"));
        }
    }
}
