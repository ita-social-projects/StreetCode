using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.Analytics;

namespace Streetcode.DAL.Configurations;

public class StatisticRecordMap : IEntityTypeConfiguration<StatisticRecord>
{
    public void Configure(EntityTypeBuilder<StatisticRecord> builder)
    {
        builder
            .HasOne(x => x.StreetcodeCoordinate)
            .WithOne(x => x.StatisticRecord)
            .HasForeignKey<StatisticRecord>(x => x.StreetcodeCoordinateId);
    }
}