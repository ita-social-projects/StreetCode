using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFTask.Entities.AdditionalContent.Coordinates;

namespace Streetcode.DAL.Entities.Toponyms;

[Table("toponyms", Schema = "toponyms")]
public class Toponym
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Title { get; set; }
    
    public string? Description { get; set; }

    public List<Streetcode.Streetcode> Streetcodes { get; set; } = new();

    public List<ToponymCoordinate> Coordinates { get; set; } = new();
}