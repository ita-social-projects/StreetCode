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
    public string? Description { get; set; }

    [Required]
    public string BlobName { get; set; }

    [Required]
    public string MimeType { get; set; }

    public StreetcodeContent? Streetcode { get; set; }
}