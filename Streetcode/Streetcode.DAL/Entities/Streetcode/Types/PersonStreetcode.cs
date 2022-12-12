using System.ComponentModel.DataAnnotations;

namespace EFTask.Entities.Streetcode.Types;

public class PersonStreetcode : Streetcode
{
    [MaxLength(50)]
    public string FirstName { get; set; }

    [MaxLength(50)]
    public string MiddleName { get; set; }

    [MaxLength(50)]
    public string LastName { get; set; }
}