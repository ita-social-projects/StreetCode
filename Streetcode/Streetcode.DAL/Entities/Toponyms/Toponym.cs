using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates;
using Streetcode.DAL.Entities.Streetcode;

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

    public List<StreetcodeContent> Streetcodes { get; set; } = new();

    public List<ToponymCoordinate> Coordinates { get; set; } = new();
}