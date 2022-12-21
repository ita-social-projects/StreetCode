using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Streetcode.DAL.Entities.Streetcode;

[Table("streetcode_timeline_item", Schema = "streetcode")]
public class StreetcodeTimelineItem
{
    [Required]
    public int StreetcodesId { get; set; }

    [Required]
    public int TimelineItemsId { get; set; }
}