using System.ComponentModel.DataAnnotations;
using Streetcode.DAL.Entities.Toponyms;

namespace Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;

public class ToponymCoordinate : Coordinate
{
    [Required]
    public int ToponymId { get; set; }

    public Toponym? Toponym { get; set; }
}