using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Streetcode.DAL.Entities.Timeline;

[Table("historical_contexts", Schema = "timeline")]
public class HistoricalContext
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string? Title { get; set; }

    public List<HistoricalContextTimeline> HistoricalContextTimelines { get; set; } = new();
}
