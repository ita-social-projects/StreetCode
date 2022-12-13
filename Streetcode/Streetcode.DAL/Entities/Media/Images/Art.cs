using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Streetcode.DAL.Entities.Media.Images;

[Table("arts", Schema = "media")]
public class Art
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string? Description { get; set; }

    public List<Streetcode.Streetcode> Streetcodes { get; set; } = new();

    public int ImageId { get; set; }

    public Image? Image { get; set; }
}
