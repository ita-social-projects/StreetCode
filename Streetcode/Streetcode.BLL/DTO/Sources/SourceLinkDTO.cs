using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Sources;

public class SourceLinkDTO
{
    public int Id { get; set; }
    public UrlDTO Url { get; set; }
    public int StreetcodeId { get; set; }
    public StreetcodeDTO? Streetcode { get; set; }
}