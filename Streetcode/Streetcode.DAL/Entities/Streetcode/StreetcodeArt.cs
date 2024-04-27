using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Media.Images;

namespace Streetcode.DAL.Entities.Streetcode;

[Table("streetcode_art", Schema = "streetcode")]
public class StreetcodeArt
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int Index { get; set; }
    public int? StreetcodeArtSlideId { get; set; }
    public int ArtId { get; set; }
    public int? StreetcodeId { get; set; }
    public Art? Art { get; set; }
    public StreetcodeArtSlide? StreetcodeArtSlide { get; set; }
    public StreetcodeContent? Streetcode { get; set; }
}