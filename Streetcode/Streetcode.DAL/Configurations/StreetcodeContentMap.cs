using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.DAL.Enums;

namespace Streetcode.DAL.Configurations;

public class StreetcodeContentMap : IEntityTypeConfiguration<StreetcodeContent>
{
    public void Configure(EntityTypeBuilder<StreetcodeContent> builder)
    {
        builder
                .Property(s => s.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

        builder
                .Property(s => s.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");

        builder
                .Property(s => s.ViewCount)
                .HasDefaultValue(0);

        builder
                .HasDiscriminator<string>(StreetcodeTypeDiscriminators.DiscriminatorName)
                .HasValue<StreetcodeContent>(StreetcodeTypeDiscriminators.StreetcodeBaseType)
                .HasValue<PersonStreetcode>(StreetcodeTypeDiscriminators.StreetcodePersonType)
                .HasValue<EventStreetcode>(StreetcodeTypeDiscriminators.StreetcodeEventType);

        builder
                .Property<string>("StreetcodeType").Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);

        builder
                .HasMany(d => d.Coordinates)
                .WithOne(c => c.Streetcode)
                .OnDelete(DeleteBehavior.Cascade);

        builder
                .HasMany(d => d.Facts)
                .WithOne(f => f.Streetcode)
                .OnDelete(DeleteBehavior.Cascade);

        builder
                .HasMany(d => d.Images)
                .WithMany(i => i.Streetcodes)
                .UsingEntity<StreetcodeImage>(
                    si => si.HasOne(i => i.Image)
                                                                        .WithMany()
                                                                        .HasForeignKey(i => i.ImageId),
                    si => si.HasOne(i => i.Streetcode)
                                                                        .WithMany()
                                                                        .HasForeignKey(i => i.StreetcodeId))
                .ToTable("streetcode_image", "streetcode");

        builder
                .HasMany(d => d.TimelineItems)
                .WithOne(t => t.Streetcode)
                .OnDelete(DeleteBehavior.Cascade);

        builder
                .HasMany(d => d.Toponyms)
                .WithMany(t => t.Streetcodes)
                .UsingEntity<StreetcodeToponym>(
                    st => st.HasOne(s => s.Toponym)
                                                                            .WithMany()
                                                                            .HasForeignKey(x => x.ToponymId),
                    st => st.HasOne(s => s.Streetcode)
                                                                            .WithMany()
                                                                            .HasForeignKey(x => x.StreetcodeId))
                .ToTable("streetcode_toponym", "streetcode");

        builder
                .HasMany(d => d.SourceLinkCategories)
                .WithMany(c => c.Streetcodes)
                .UsingEntity<StreetcodeCategoryContent>(
                        scat => scat.HasOne(i => i.SourceLinkCategory)
                                                                                        .WithMany(s => s.StreetcodeCategoryContents)
                                                                                        .HasForeignKey(i => i.SourceLinkCategoryId),
                        scat => scat.HasOne(i => i.Streetcode)
                                                                                        .WithMany(s => s.StreetcodeCategoryContents)
                                                                                        .HasForeignKey(i => i.StreetcodeId))
                .ToTable("streetcode_source_link_categories", "sources");

        builder
                .HasMany(d => d.Partners)
                .WithMany(p => p.Streetcodes)
                .UsingEntity<StreetcodePartner>(
                        sp => sp.HasOne(i => i.Partner)
                                                                         .WithMany()
                                                                         .HasForeignKey(x => x.PartnerId),
                        sp => sp.HasOne(i => i.Streetcode)
                                                                        .WithMany()
                                                                        .HasForeignKey(x => x.StreetcodeId))
                .ToTable("streetcode_partners", "streetcode");

        builder
                .HasMany(d => d.Videos)
                .WithOne(p => p.Streetcode)
                .HasForeignKey(d => d.StreetcodeId)
                .OnDelete(DeleteBehavior.Cascade);

        builder
                .HasOne(d => d.Audio)
                .WithOne(p => p.Streetcode)
                .HasForeignKey<StreetcodeContent>(d => d.AudioId)
                .OnDelete(DeleteBehavior.SetNull);

        builder
                .HasOne(d => d.Text)
                .WithOne(p => p.Streetcode)
                .HasForeignKey<Text>(d => d.StreetcodeId)
                .OnDelete(DeleteBehavior.Cascade);

        builder
                .HasOne(d => d.TransactionLink)
                .WithOne(p => p.Streetcode)
                .HasForeignKey<TransactionLink>(d => d.StreetcodeId)
                .OnDelete(DeleteBehavior.Cascade);

        builder
                .HasMany(d => d.StatisticRecords)
                .WithOne(t => t.Streetcode)
                .HasForeignKey(t => t.StreetcodeId)
                .OnDelete(DeleteBehavior.NoAction);
    }
}