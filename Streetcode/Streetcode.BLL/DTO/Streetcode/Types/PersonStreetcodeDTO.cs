namespace Streetcode.BLL.DTO.Streetcode.Types;

public class PersonStreetcodeDto : StreetcodeDto
{
    public string FirstName { get; set; } = null!;
    public string? Rank { get; set; }
    public string LastName { get; set; } = null!;
}