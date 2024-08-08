using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Analytics;
using Streetcode.DAL.Entities.Feedback;
using Streetcode.DAL.Entities.Jobs;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.DAL.Entities.Users;
using StreetcodeArtSlide = Streetcode.DAL.Entities.Streetcode.StreetcodeArtSlide;

namespace Streetcode.DAL.Persistence;

public class StreetcodeDbContext : IdentityDbContext<User>
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
    public DbSet<StreetcodeArtSlide> StreetcodeArtSlides { get; set; }
    public override DbSet<User> Users { get; set; }
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
    public DbSet<Job> Job { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.UseCollation("SQL_Ukrainian_CP1251_CI_AS");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
