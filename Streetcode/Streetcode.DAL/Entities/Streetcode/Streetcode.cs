using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFTask.Entities.AdditionalContent;
using EFTask.Entities.AdditionalContent.Coordinates;
using EFTask.Entities.Media;
using EFTask.Entities.Media.Images;
using EFTask.Entities.Partners;
using EFTask.Entities.Sources;
using EFTask.Entities.Streetcode.TextContent;
using EFTask.Entities.Timeline;
using EFTask.Entities.Toponyms;
using EFTask.Entities.Transactions;

namespace EFTask.Entities.Streetcode;

[Table("streetcodes", Schema = "streetcode")]
public class Streetcode
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int Index { get; set; }
    
    [Required]
    public string Teaser { get; set; }
       
    public int ViewCount { get; set; }

    public DateTime CreateDate { get; set; }
    
    public DateTime UpdateDate { get; set; }

    [Required]
    public DateTime EventStartOrPersonBirthDate { get; set; }

    [Required]
    public DateTime EventEndOrPersonDeathDate { get; set; }

    public Text? Text { get; set; }

    public Audio? Audio { get; set; }

    public StreetcodeCoordinate? Coordinate { get; set; }

    public TransactionLink? TransactionLink { get; set; }

    public List<Toponym> Toponyms { get; set; } = new();

    public List<Art> Arts { get; set; } = new();
    
    public List<Image> Images { get; set; } = new();
    
    public List<Tag> Tags { get; set; } = new();

    public List<Subtitle> Subtitles { get; set; } = new();
    
    public List<Fact> Facts { get; set; } = new();
    
    public List<Video> Videos { get; set; } = new();
    
    public List<SourceLink> SourceLinks { get; set; } = new();

    public List<TimelineItem> TimelineItems { get; set; } = new();

    public List<RelatedFigure> Observers { get; set; } = new();
    
    public List<RelatedFigure> Targets { get; set; } = new();

    public List<StreetcodePartner> StreetcodePartners { get; set; } = new();
}