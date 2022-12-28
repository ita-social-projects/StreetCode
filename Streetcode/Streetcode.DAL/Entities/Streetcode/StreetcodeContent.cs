using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Entities.Transactions;

namespace Streetcode.DAL.Entities.Streetcode;

[Table("streetcodes", Schema = "streetcode")]
public class StreetcodeContent
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int Index { get; set; }

    [Required]
    public string Teaser { get; set; }

    public int ViewCount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [Required]
    public DateTime EventStartOrPersonBirthDate { get; set; }

    [Required]
    public DateTime EventEndOrPersonDeathDate { get; set; }

    public Text? Text { get; set; }

    public Audio? Audio { get; set; }

    public StreetcodeCoordinate? Coordinate { get; set; }

    public TransactionLink? TransactionLink { get; set; }

    public List<Toponym> Toponyms { get; set; } = new ();

    public List<Art> Arts { get; set; } = new ();

    public List<Image> Images { get; set; } = new ();

    public List<Tag> Tags { get; set; } = new ();

    public List<Subtitle> Subtitles { get; set; } = new ();

    public List<Fact> Facts { get; set; } = new ();

    public List<Video> Videos { get; set; } = new ();

    public List<SourceLink> SourceLinks { get; set; } = new ();

    public List<TimelineItem> TimelineItems { get; set; } = new ();

    public List<RelatedFigure> Observers { get; set; } = new ();

    public List<RelatedFigure> Targets { get; set; } = new ();

    public List<StreetcodePartner> StreetcodePartners { get; set; } = new ();
}