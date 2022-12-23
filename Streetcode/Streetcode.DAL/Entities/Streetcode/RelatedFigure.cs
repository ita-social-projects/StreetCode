using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Streetcode.DAL.Entities.Streetcode;

[Table("related_figures", Schema = "streetcode")]
public class RelatedFigure
{
    [Required]
    public int ObserverId { get; set; }

    public StreetcodeContent Observer { get; set; }

    [Required]
    public int TargetId { get; set; }

    public StreetcodeContent Target { get; set; }
}