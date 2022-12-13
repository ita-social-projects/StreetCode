using System.ComponentModel.DataAnnotations;
using EFTask.Entities.AdditionalContent.Coordinates;

namespace Streetcode.DAL.Entities.AdditionalContent.Coordinates;

public class StreetcodeCoordinate : Coordinate
{
    [Required]
    public int StreetcodeId { get; set; }

    public Streetcode.Streetcode? Streetcode { get; set; }
}