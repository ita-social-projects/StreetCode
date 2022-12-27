using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.AdditionalContent.Subtitles;

public class SubtitleDTO
{
    public int Id { get; set; }
    public SubtitleStatusDTO SubtitleStatus { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Description { get; set; }
    public UrlDTO? Url { get; set; }
    public int StreetcodeId { get; set; }
    public StreetcodeDTO Streetcode { get; set; }
}