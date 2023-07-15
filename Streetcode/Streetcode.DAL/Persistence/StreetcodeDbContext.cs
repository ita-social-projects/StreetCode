using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Analytics;
using Streetcode.DAL.Entities.Feedback;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Enums;

namespace Streetcode.DAL.Persistence;

public class StreetcodeDbContext : DbContext
{
    public StreetcodeDbContext()
    {
    }

    public StreetcodeDbContext(DbContextOptions<StreetcodeDbContext> options)
        : base(options)
    {
    }

    public DbSet<Art> Arts { get; set; }
    public DbSet<Audio> Audios { get; set; }
    public DbSet<ToponymCoordinate> ToponymCoordinates { get; set; }
    public DbSet<StreetcodeCoordinate> StreetcodeCoordinates { get; set; }
    public DbSet<Fact> Facts { get; set; }
    public DbSet<HistoricalContext> HistoricalContexts { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<ImageDetails> ImageDetailses { get; set; }
    public DbSet<Partner> Partners { get; set; }
    public DbSet<PartnerSourceLink> PartnerSourceLinks { get; set; }
    public DbSet<RelatedFigure> RelatedFigures { get; set; }
    public DbSet<Response> Responses { get; set; }
    public DbSet<StreetcodeContent> Streetcodes { get; set; }
    public DbSet<Subtitle> Subtitles { get; set; }
    public DbSet<StatisticRecord> StatisticRecords { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Term> Terms { get; set; }
    public DbSet<RelatedTerm> RelatedTerms { get; set; }
    public DbSet<Text> Texts { get; set; }
    public DbSet<TimelineItem> TimelineItems { get; set; }
    public DbSet<Toponym> Toponyms { get; set; }
    public DbSet<TransactionLink> TransactionLinks { get; set; }
    public DbSet<Video> Videos { get; set; }
    public DbSet<StreetcodeCategoryContent> StreetcodeCategoryContent { get; set; }
    public DbSet<StreetcodeArt> StreetcodeArts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<StreetcodeTagIndex> StreetcodeTagIndices { get; set; }
    public DbSet<TeamMember> TeamMembers { get; set; }
    public DbSet<TeamMemberLink> TeamMemberLinks { get; set; }
    public DbSet<Positions> Positions { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<SourceLinkCategory> SourceLinks { get; set; }
    public DbSet<StreetcodeImage> StreetcodeImages { get; set; }
    public DbSet<HistoricalContextTimeline> HistoricalContextsTimelines { get; set; }
    public DbSet<StreetcodePartner> StreetcodePartners { get; set; }
    public DbSet<TeamMemberPositions> TeamMemberPosition { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.UseCollation("SQL_Ukrainian_CP1251_CI_AS");

        modelBuilder.Entity<StatisticRecord>()
              .HasOne(x => x.StreetcodeCoordinate)
              .WithOne(x => x.StatisticRecord)
              .HasForeignKey<StatisticRecord>(x => x.StreetcodeCoordinateId);

        modelBuilder.Entity<News>()
            .HasOne(x => x.Image)
            .WithOne(x => x.News)
            .HasForeignKey<News>(x => x.ImageId);

        modelBuilder.Entity<TeamMember>()
            .HasOne(x => x.Image)
            .WithOne(x => x.TeamMember)
            .HasForeignKey<TeamMember>(x => x.ImageId);

        modelBuilder.Entity<TeamMember>()
            .HasMany(x => x.Positions)
            .WithMany(x => x.TeamMembers)
            .UsingEntity<TeamMemberPositions>(
            tp => tp.HasOne(x => x.Positions).WithMany().HasForeignKey(x => x.PositionsId),
            tp => tp.HasOne(x => x.TeamMember).WithMany().HasForeignKey(x => x.TeamMemberId));

        modelBuilder.Entity<TeamMember>()
            .HasMany(x => x.TeamMemberLinks)
            .WithOne(x => x.TeamMember)
            .HasForeignKey(x => x.TeamMemberId);

        modelBuilder.Entity<TeamMemberPositions>()
            .HasKey(nameof(TeamMemberPositions.TeamMemberId), nameof(TeamMemberPositions.PositionsId));

        modelBuilder.Entity<Tag>()
            .HasMany(t => t.Streetcodes)
            .WithMany(s => s.Tags)
            .UsingEntity<StreetcodeTagIndex>(
            sp => sp.HasOne(x => x.Streetcode).WithMany(x => x.StreetcodeTagIndices).HasForeignKey(x => x.StreetcodeId),
            sp => sp.HasOne(x => x.Tag).WithMany(x => x.StreetcodeTagIndices).HasForeignKey(x => x.TagId));

        modelBuilder.Entity<StreetcodeTagIndex>()
           .HasKey(nameof(StreetcodeTagIndex.StreetcodeId), nameof(StreetcodeTagIndex.TagId));

        modelBuilder.Entity<Toponym>()
            .HasOne(d => d.Coordinate)
            .WithOne(p => p.Toponym)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Partner>(entity =>
        {
            entity.HasMany(d => d.PartnerSourceLinks)
                .WithOne(p => p.Partner)
                .HasForeignKey(d => d.PartnerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(p => p.IsKeyPartner)
                .HasDefaultValue("false");
        });

        modelBuilder.Entity<HistoricalContextTimeline>()
             .HasKey(ht => new { ht.TimelineId, ht.HistoricalContextId });
        modelBuilder.Entity<HistoricalContextTimeline>()
            .HasOne(ht => ht.Timeline)
            .WithMany(x => x.HistoricalContextTimelines)
            .HasForeignKey(x => x.TimelineId);
        modelBuilder.Entity<HistoricalContextTimeline>()
            .HasOne(ht => ht.HistoricalContext)
            .WithMany(x => x.HistoricalContextTimelines)
            .HasForeignKey(x => x.HistoricalContextId);

        modelBuilder.Entity<SourceLinkCategory>()
            .HasMany(d => d.StreetcodeCategoryContents)
            .WithOne(p => p.SourceLinkCategory)
            .HasForeignKey(d => d.SourceLinkCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasOne(d => d.Art)
                .WithOne(a => a.Image)
                .HasForeignKey<Art>(a => a.ImageId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(im => im.ImageDetails)
                .WithOne(info => info.Image)
                .HasForeignKey<ImageDetails>(a => a.ImageId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Partner)
                .WithOne(p => p.Logo)
                .HasForeignKey<Partner>(d => d.LogoId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(d => d.Facts)
                .WithOne(p => p.Image)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(i => i.SourceLinkCategories)
                .WithOne(s => s.Image)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RelatedFigure>(entity =>
        {
            entity.HasKey(d => new { d.ObserverId, d.TargetId });

            entity.HasOne(d => d.Observer)
                .WithMany(d => d.Observers)
                .HasForeignKey(d => d.ObserverId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Target)
                .WithMany(d => d.Targets)
                .HasForeignKey(d => d.TargetId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<StreetcodeArt>(entity =>
        {
            entity.HasKey(d => new { d.ArtId, d.StreetcodeId });

            entity.HasOne(d => d.Streetcode)
                .WithMany(d => d.StreetcodeArts)
                .HasForeignKey(d => d.StreetcodeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Art)
                .WithMany(d => d.StreetcodeArts)
                .HasForeignKey(d => d.ArtId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.Index)
                .HasDefaultValue(1);

            entity
                .HasIndex(d => new { d.ArtId, d.StreetcodeId })
                .IsUnique(false);
        });

        modelBuilder.Entity<StreetcodeContent>(entity =>
        {
            entity.Property(s => s.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            entity.Property(s => s.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");

            entity.Property(s => s.ViewCount)
                .HasDefaultValue(0);

            entity.HasDiscriminator<string>(StreetcodeTypeDiscriminators.DiscriminatorName)
                .HasValue<StreetcodeContent>(StreetcodeTypeDiscriminators.StreetcodeBaseType)
                .HasValue<PersonStreetcode>(StreetcodeTypeDiscriminators.StreetcodePersonType)
                .HasValue<EventStreetcode>(StreetcodeTypeDiscriminators.StreetcodeEventType);

            entity.Property<string>("StreetcodeType").Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);

            entity.HasMany(d => d.Coordinates)
                .WithOne(c => c.Streetcode)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(d => d.Facts)
                .WithOne(f => f.Streetcode)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(d => d.Images)
                .WithMany(i => i.Streetcodes)
                .UsingEntity<StreetcodeImage>(
                    si => si.HasOne(i => i.Image).WithMany().HasForeignKey(i => i.ImageId),
                    si => si.HasOne(i => i.Streetcode).WithMany().HasForeignKey(i => i.StreetcodeId))
                .ToTable("streetcode_image", "streetcode");

            entity.HasMany(d => d.TimelineItems)
                .WithOne(t => t.Streetcode)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(d => d.Toponyms)
                .WithMany(t => t.Streetcodes)
                .UsingEntity<StreetcodeToponym>(
                    st => st.HasOne(s => s.Toponym).WithMany().HasForeignKey(x => x.ToponymId),
                    st => st.HasOne(s => s.Streetcode).WithMany().HasForeignKey(x => x.StreetcodeId))
                .ToTable("streetcode_toponym", "streetcode");

            entity.HasMany(d => d.SourceLinkCategories)
                    .WithMany(c => c.Streetcodes)
                    .UsingEntity<StreetcodeCategoryContent>(
                        scat => scat.HasOne(i => i.SourceLinkCategory).WithMany(s => s.StreetcodeCategoryContents).HasForeignKey(i => i.SourceLinkCategoryId),
                        scat => scat.HasOne(i => i.Streetcode).WithMany(s => s.StreetcodeCategoryContents).HasForeignKey(i => i.StreetcodeId))
                    .ToTable("streetcode_source_link_categories", "sources");

            entity.HasMany(d => d.Partners)
                    .WithMany(p => p.Streetcodes)
                    .UsingEntity<StreetcodePartner>(
                        sp => sp.HasOne(i => i.Partner).WithMany().HasForeignKey(x => x.PartnerId),
                        sp => sp.HasOne(i => i.Streetcode).WithMany().HasForeignKey(x => x.StreetcodeId))
                   .ToTable("streetcode_partners", "streetcode");

            entity.HasMany(d => d.Videos)
                    .WithOne(p => p.Streetcode)
                    .HasForeignKey(d => d.StreetcodeId)
                    .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Audio)
                    .WithOne(p => p.Streetcode)
                    .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Text)
                    .WithOne(p => p.Streetcode)
                    .HasForeignKey<Text>(d => d.StreetcodeId)
                    .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.TransactionLink)
                    .WithOne(p => p.Streetcode)
                    .HasForeignKey<TransactionLink>(d => d.StreetcodeId)
                    .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(d => d.StatisticRecords)
                    .WithOne(t => t.Streetcode)
                    .HasForeignKey(t => t.StreetcodeId)
                    .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<RelatedTerm>()
            .HasOne(rt => rt.Term)
            .WithMany(t => t.RelatedTerms)
            .HasForeignKey(rt => rt.TermId);

        modelBuilder.Entity<Coordinate>()
            .HasDiscriminator<string>("CoordinateType")
            .HasValue<Coordinate>("coordinate_base")
            .HasValue<StreetcodeCoordinate>("coordinate_streetcode")
            .HasValue<ToponymCoordinate>("coordinate_toponym");
    }
}
