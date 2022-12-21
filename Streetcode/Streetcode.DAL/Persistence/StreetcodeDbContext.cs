using Streetcode.DAL.Entities.AdditionalContent.Coordinates;
using Streetcode.DAL.Entities.Feedback;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Extensions;
using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.DAL.Repositories.Interfaces.Streetcode;

namespace Streetcode.DAL.Persistence;

public class StreetcodeDbContext : DbContext
{
    public StreetcodeDbContext()
    {
    }

    public StreetcodeDbContext(DbContextOptions<StreetcodeDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Art> Arts { get; set; }
    public virtual DbSet<Audio> Audios { get; set; }
    public virtual DbSet<ToponymCoordinate> ToponymCoordinates { get; set; }
    public virtual DbSet<StreetcodeCoordinate> StreetcodeCoordinates { get; set; }
    public virtual DbSet<Fact> Facts { get; set; }
    public virtual DbSet<HistoricalContext> HistoricalContexts { get; set; }
    public virtual DbSet<Image> Images { get; set; }
    public virtual DbSet<Partner> Partners { get; set; }
    public virtual DbSet<PartnerSourceLink> PartnerSourceLinks { get; set; }
    public virtual DbSet<RelatedFigure> RelatedFigures { get; set; }
    public virtual DbSet<Response> Responses { get; set; }
    public virtual DbSet<SourceLink> SourceLinks { get; set; }
    public virtual DbSet<Entities.Streetcode.StreetcodeContent> Streetcodes { get; set; }
    public virtual DbSet<StreetcodeTimelineItem> StreetcodeTimelineItems { get; set; }
    public virtual DbSet<Subtitle> Subtitles { get; set; }
    public virtual DbSet<Tag> Tags { get; set; }
    public virtual DbSet<Term> Terms { get; set; }
    public virtual DbSet<Text> Texts { get; set; }
    public virtual DbSet<TimelineItem> TimelineItems { get; set; }
    public virtual DbSet<Toponym> Toponyms { get; set; }
    public virtual DbSet<TransactionLink> TransactionLinks { get; set; }
    public virtual DbSet<Video> Videos { get; set; }
    public virtual DbSet<SourceLinkCategory> SourceLinkCategories { get; set; }
    public virtual DbSet<StreetcodePartner> StreetcodePartners { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Toponym>()
            .HasMany(d => d.Coordinates)
            .WithOne(p => p.Toponym)
            .HasForeignKey(d => d.ToponymId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Partner>()
            .HasMany(d => d.PartnerSourceLinks)
            .WithOne(p => p.Partner)
            .HasForeignKey(d => d.PartnerId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<TimelineItem>()
            .HasMany(d => d.HistoricalContexts)
            .WithMany(h => h.TimelineItems)
            .UsingEntity(j => j.ToTable("timeline_item_historical_context", "timeline"));

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasOne(d => d.Art)
                .WithOne(a => a.Image)
                .HasForeignKey<Art>(a => a.ImageId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(d => d.Facts)
                .WithOne(p => p.Image)
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

        modelBuilder.Entity<StreetcodePartner>(entity =>
        {
            entity.HasKey(d => new { d.PartnerId, d.StreetcodeId });

            entity.HasOne(d => d.StreetCode)
                .WithMany(d => d.StreetcodePartners)
                .HasForeignKey(d => d.StreetcodeId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(d => d.Partner)
                .WithMany(d => d.StreetcodePartners)
                .HasForeignKey(d => d.PartnerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.IsSponsor)
                .HasDefaultValue(false);
        });

        modelBuilder.Entity<StreetcodeTimelineItem>(entity =>
        {
            entity.HasKey(k => new { k.StreetcodesId, k.TimelineItemsId });
        });

        modelBuilder.Entity<Entities.Streetcode.StreetcodeContent>(entity =>
        {
            entity.Property(s => s.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
            entity.Property(s => s.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");
            entity.Property(s => s.ViewCount)
                .HasDefaultValue(0);
            entity.HasDiscriminator<string>("streetcode_type")
                .HasValue<Entities.Streetcode.StreetcodeContent>("streetcode_base")
                .HasValue<PersonStreetCode>("streetcode_person")
                .HasValue<EventStreetCode>("streetcode_event");

            entity.HasOne(d => d.Coordinate)
                .WithOne(c => c.Streetcode)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(d => d.Arts)
                .WithMany(a => a.Streetcodes)
                .UsingEntity(j => j.ToTable("streetcode_arts", "streetcode"));

            entity.HasMany(d => d.Facts)
                .WithMany(f => f.Streetcodes)
                .UsingEntity(j => j.ToTable("streetcode_fact", "streetcode"));

            entity.HasMany(d => d.Tags)
                .WithMany(t => t.Streetcodes)
                .UsingEntity(j => j.ToTable("streetcode_tag", "streetcode"));
            entity.HasMany(d => d.Images)
                .WithMany(i => i.Streetcodes)
                .UsingEntity(j => j.ToTable("streetcode_image", "streetcode"));
            entity.HasMany(d => d.Toponyms)
                .WithMany(t => t.Streetcodes)
                .UsingEntity(j => j.ToTable("streetcode_toponym", "streetcode"));

            entity.HasMany(d => d.SourceLinks)
                .WithOne(p => p.Streetcode)
                .HasForeignKey(d => d.StreetcodeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(d => d.Videos)
                .WithOne(p => p.Streetcode)
                .HasForeignKey(d => d.StreetcodeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Audio)
                .WithOne(p => p.Streetcode)
                .HasForeignKey<Audio>(d => d.StreetcodeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Text)
                .WithOne(p => p.Streetcode)
                .HasForeignKey<Text>(d => d.StreetcodeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.TransactionLink)
                .WithOne(p => p.Streetcode)
                .HasForeignKey<TransactionLink>(d => d.StreetcodeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Coordinate>()
            .HasDiscriminator<string>("coordinate_type")
            .HasValue<Coordinate>("coordinate_base")
            .HasValue<StreetcodeCoordinate>("coordinate_streetcode")
            .HasValue<ToponymCoordinate>("coordinate_toponym");

        modelBuilder.SeedData();
    }
}