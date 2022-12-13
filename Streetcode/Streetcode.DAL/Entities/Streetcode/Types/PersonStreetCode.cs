using System.ComponentModel.DataAnnotations;

namespace Streetcode.DAL.Entities.Streetcode.Types;

public class PersonStreetCode : StreetcodeContent
{
    [MaxLength(50)]
    public string FirstName { get; set; }

    [MaxLength(50)]
    public string MiddleName { get; set; }

    [MaxLength(50)]
    public string LastName { get; set; }
}