using System.ComponentModel.DataAnnotations;

namespace Streetcode.DAL.Entities.Streetcode.Types;

public class EventStreetCode : StreetcodeContent
{
    [MaxLength(100)]
    public string Title { get; set; }
}