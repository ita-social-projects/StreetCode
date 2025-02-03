namespace Streetcode.BLL.DTO.AdditionalContent.Subtitles;

public class SubtitleDto : SubtitleCreateUpdateDto
{
    public int Id { get; set; }
    public int StreetcodeId { get; set; }
}
