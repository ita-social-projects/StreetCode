using System.ComponentModel.DataAnnotations;

namespace EFTask.Entities.AdditionalContent.Coordinates;

public class StreetcodeCoordinate : Coordinate
{
    [Required]
    public int StreetcodeId { get; set; }

    public Streetcode.Streetcode? Streetcode { get; set; }
}