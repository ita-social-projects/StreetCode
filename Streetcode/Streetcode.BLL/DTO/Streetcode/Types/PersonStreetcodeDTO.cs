namespace Streetcode.BLL.DTO.Streetcode.Types;

public class PersonStreetcodeDTO
{
    public string FirstName { get; set; }
    public string? Rank { get; set; }
    public string LastName { get; set; }
    public StreetcodeDTO Streetcode { get; set; }
}