using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.AdditionalContent;

public class SubtitleDTO
{
    public int Id;
    public StatusDTO Status;
    public string FirstName;
    public string LastName;
    public string Description;
    public UrlDTO Url;
    public int StreetcodeId;
    public StreetcodeDTO Streetcode;
}