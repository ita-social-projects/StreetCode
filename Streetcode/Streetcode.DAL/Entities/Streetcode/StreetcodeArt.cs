using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Media.Images;

namespace Streetcode.DAL.Entities.Streetcode;

[Table("streetcode_art", Schema = "streetcode")]
public class StreetcodeArt
{
    public int Index { get; set; }

    [Required]
    public int StreetcodeId { get; set; }

    public StreetcodeContent? Streetcode { get; set; }

    [Required]
    public int ArtId { get; set; }

    public Art? Art { get; set; }
}