using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Feedback;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.DAL.Extensions;

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
    public DbSet<Partner> Partners { get; set; }
    public DbSet<PartnerSourceLink> PartnerSourceLinks { get; set; }
    public DbSet<RelatedFigure> RelatedFigures { get; set; }
    public DbSet<Response> Responses { get; set; }
    public DbSet<Donation> Donations { get; set; }
    public DbSet<SourceLink> SourceLinks { get; set; }
    public DbSet<StreetcodeContent> Streetcodes { get; set; }
    public DbSet<Subtitle> Subtitles { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Term> Terms { get; set; }
    public DbSet<RelatedTerm> RelatedTerms { get; set; }
    public DbSet<Text> Texts { get; set; }
    public DbSet<TimelineItem> TimelineItems { get; set; }
    public DbSet<Toponym> Toponyms { get; set; }
    public DbSet<TransactionLink> TransactionLinks { get; set; }
    public DbSet<Video> Videos { get; set; }
    public DbSet<SourceLinkCategory> SourceLinkCategories { get; set; }
    public DbSet<SourceLinkSubCategory> SourceLinkSubCategories { get; set; }
    public DbSet<StreetcodeArt> StreetcodeArts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.UseCollation("SQL_Ukrainian_CP1251_CI_AS");

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

        modelBuilder.Entity<TimelineItem>()
            .HasMany(d => d.HistoricalContexts)
            .WithMany(h => h.TimelineItems)
            .UsingEntity(j => j.ToTable("timeline_item_historical_context", "timeline"));

        modelBuilder.Entity<SourceLinkCategory>()
            .HasMany(d => d.SubCategories)
            .WithOne(p => p.SourceLinkCategory)
            .HasForeignKey(d => d.SourceLinkCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SourceLink>()
            .HasMany(d => d.SubCategories)
            .WithMany(h => h.SourceLinks)
            .UsingEntity(j => j.ToTable("source_link_source_link_subcategory", "sources"));

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasOne(d => d.Art)
                .WithOne(a => a.Image)
                .HasForeignKey<Art>(a => a.ImageId)
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
                .OnDelete(DeleteBehavior.Cascade);

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

            entity.HasDiscriminator<string>("StreetcodeType")
                .HasValue<StreetcodeContent>("streetcode-base")
                .HasValue<PersonStreetcode>("streetcode-person")
                .HasValue<EventStreetcode>("streetcode-event");

            entity.HasMany(d => d.Coordinates)
                .WithOne(c => c.Streetcode)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(d => d.Facts)
                .WithMany(f => f.Streetcodes)
                .UsingEntity(j => j.ToTable("streetcode_fact", "streetcode"));

            entity.HasMany(d => d.Tags)
                .WithMany(t => t.Streetcodes)
                .UsingEntity(j => j.ToTable("streetcode_tag", "streetcode"));

            entity.HasMany(d => d.Images)
                .WithMany(i => i.Streetcodes)
                .UsingEntity(j => j.ToTable("streetcode_image", "streetcode"));

            entity.HasMany(d => d.TimelineItems)
                .WithMany(t => t.Streetcodes)
                .UsingEntity(j => j.ToTable("streetcode_timeline_item", "streetcode"));

            entity.HasMany(d => d.Toponyms)
                .WithMany(t => t.Streetcodes)
                .UsingEntity(j => j.ToTable("streetcode_toponym", "streetcode"));

            entity.HasMany(d => d.SourceLinkCategories)
                .WithMany(c => c.Streetcodes)
                .UsingEntity(j => j.ToTable("streetcode_source_link_categories", "sources"));

            entity.HasMany(d => d.Partners)
                .WithMany(p => p.Streetcodes)
                .UsingEntity(j => j.ToTable("streetcode_partners", "streetcode"));

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

        modelBuilder.Entity<RelatedTerm>()
            .HasOne(rt => rt.Term)
            .WithMany(t => t.RelatedTerms)
            .HasForeignKey(rt => rt.TermId);

        modelBuilder.Entity<Coordinate>()
            .HasDiscriminator<string>("CoordinateType")
            .HasValue<Coordinate>("coordinate_base")
            .HasValue<StreetcodeCoordinate>("coordinate_streetcode")
            .HasValue<ToponymCoordinate>("coordinate_toponym");

        modelBuilder.SeedData();
    }
}