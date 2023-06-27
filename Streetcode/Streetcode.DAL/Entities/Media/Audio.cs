using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.Media;

[Table("audios", Schema = "media")]
public class Audio
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(100)]
    public string? Title { get; set; }

    [Required]
    [MaxLength(100)]
    public string? BlobName { get; set; }

    [Required]
    [MaxLength(10)]
    public string? MimeType { get; set; }

    [NotMapped]
    public string? Base64 { get; set; }

    public StreetcodeContent? Streetcode { get; set; }
}