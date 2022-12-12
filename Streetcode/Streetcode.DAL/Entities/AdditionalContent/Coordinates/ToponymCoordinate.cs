using System.ComponentModel.DataAnnotations;
using EFTask.Entities.Toponyms;

namespace EFTask.Entities.AdditionalContent.Coordinates;

public class ToponymCoordinate : Coordinate
{
    [Required]
    public int ToponymId { get; set; }

    public Toponym? Toponym { get; set; }
}