using System.ComponentModel.DataAnnotations;

namespace Streetcode.DAL.Entities.Streetcode.Types;

public class EventStreetcode : StreetcodeContent
{
    [MaxLength(100)]
    public string Title { get; set; }
}