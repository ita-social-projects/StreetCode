using System.ComponentModel.DataAnnotations;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.AdditionalContent.Coordinates;

public class StreetcodeCoordinate : Coordinate
{
    [Required]
    public int StreetcodeId { get; set; }

    public StreetcodeContent? Streetcode { get; set; }
}