using System.ComponentModel.DataAnnotations;

namespace Streetcode.DAL.Entities.Streetcode.Types;

public class GeneralStreetode : StreetcodeContent
{
    [MaxLength(100)]
    public string? Title { get; set; }

    [MaxLength(50)]
    public string? FirstName { get; set; }

    [MaxLength(50)]
    public string? Rank { get; set; }

    [MaxLength(50)]
    public string? LastName { get; set; }
}
