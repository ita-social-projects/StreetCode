using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Entities.Event;

namespace Streetcode.DAL.Configurations
{
    public class HistoricalEventMap : IEntityTypeConfiguration<HistoricalEvent>
    {
        public void Configure(EntityTypeBuilder<HistoricalEvent> builder)
        {
            builder.HasOne(e => e.TimelineItem)
                .WithMany()
                .HasForeignKey("TimelineItemId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
