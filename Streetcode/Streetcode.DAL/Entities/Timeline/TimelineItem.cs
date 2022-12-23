using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.Timeline;

[Table("timeline_items", Schema = "timeline")]
public class TimelineItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    public string? Description { get; set; }

    public List<StreetcodeContent> Streetcodes { get; set; } = new ();

    public List<HistoricalContext> HistoricalContexts { get; set; } = new ();
}