using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.Media.Images;

namespace Streetcode.DAL.Entities.Streetcode;

[Table("streetcode_art_slide", Schema = "streetcode")]
public class StreetcodeArtSlide
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int StreetcodeId { get; set; }

    [Required]
    public Enums.StreetcodeArtSlideTemplate Template { get; set; }

    public StreetcodeContent? Streetcode { get; set; }

    public int Index { get; set; }

    public List<StreetcodeArt>? StreetcodeArts { get; set; } = new ();
}