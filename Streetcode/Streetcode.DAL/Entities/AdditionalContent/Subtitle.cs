using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Enums;

namespace Streetcode.DAL.Entities.AdditionalContent;

[Table("subtitles", Schema = "add_content")]
public class Subtitle
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public SubtitleStatus Status { get; set; } = default;

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? Description { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? Url { get; set; }

    [Required]
    public int StreetcodeId { get; set; }

    public Streetcode.StreetcodeContent? Streetcode { get; set; }
}
