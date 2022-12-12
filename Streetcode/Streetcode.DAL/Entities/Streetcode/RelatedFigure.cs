using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFTask.Entities.Streetcode;

[Table("related_figures", Schema = "streetcode")]
public class RelatedFigure
{
    [Required]
    public int ObserverId { get; set; }

    public Streetcode Observer { get; set; }
    
    [Required]
    public int TargetId { get; set; }
    
    public Streetcode Target { get; set; }
}