using System.ComponentModel.DataAnnotations;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates;

namespace Streetcode.DAL.Entities.AdditionalContent.Coordinates;

public class StreetcodeCoordinate : Coordinate
{
    [Required]
    public int StreetcodeId { get; set; }

    public Streetcode.StreetcodeContent? Streetcode { get; set; }
}