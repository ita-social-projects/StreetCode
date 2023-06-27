using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Streetcode.DAL.Entities.Streetcode.TextContent;

[Table("texts", Schema = "streetcode")]
public class Text
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(300)]
    public string? Title { get; set; }
    [Required]
    [MaxLength(15000)]
    public string? TextContent { get; set; }
    [MaxLength(500)]
    public string? AdditionalText { get; set; }
    [Required]
    public int StreetcodeId { get; set; }
    public StreetcodeContent? Streetcode { get; set; }
}