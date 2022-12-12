using System.ComponentModel.DataAnnotations;

namespace EFTask.Entities.Streetcode.Types;

public class EventStreetcode : Streetcode
{
    [MaxLength(100)]
    public string Title { get; set; }
}