using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Media.Images;

namespace Streetcode.DAL.Entities.Streetcode.TextContent;

[Table("facts", Schema = "streetcode")]
public class Fact
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Title { get; set; }

    [Required]
    [MaxLength(600)]
    public string? FactContent { get; set; }

    public int? ImageId { get; set; }

    public Image? Image { get; set; }

    public int StreetcodeId { get; set; }

    public StreetcodeContent? Streetcode { get; set; }
}
