using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFTask.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Streetcode.TextContent;

namespace Streetcode.DAL.Entities.Media.Images;

[Table("images", Schema = "media")]
public class Image
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(100)] 
    public string? Title { get; set; }

    [MaxLength(100)] 
    public string? Alt { get; set; }

    [Required] 
    public string Url { get; set; }

    public List<Streetcode.Streetcode> Streetcodes { get; set; } = new();

    public List<Fact> Facts { get; set; } = new();

    public Art? Art { get; set; }
}
