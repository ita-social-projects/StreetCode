using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFTask.Entities.Media;

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
    public string Url { get; set; }

    public int StreetcodeId { get; set; }

    public Streetcode.Streetcode? Streetcode { get; set; }
}