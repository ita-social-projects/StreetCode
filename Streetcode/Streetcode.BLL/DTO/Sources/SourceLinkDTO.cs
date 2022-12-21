using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Sources;

public class SourceLinkDTO
{
    public int Id;
    public string Title;
    public UrlDTO Url;
    public int StreetcodeId;
    public StreetcodeDTO Streetcode;
}