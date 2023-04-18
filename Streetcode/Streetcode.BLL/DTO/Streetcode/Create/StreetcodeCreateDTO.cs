using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Streetcode.Create.TextContent;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Streetcode.Create
{
  public class StreetcodeCreateDTO
  {
    public int Index { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Title { get; set; }
    public string DateString { get; set; }
    public string Alias { get; set; }
    public string TransliterationUrl { get; set; }
    public StreetcodeType StreetcodeType { get; set; }
    public StreetcodeStatus Status { get; set; }
    public DateTime EventStartOrPersonBirthDate { get; set; }
    public DateTime EventEndOrPersonDeathDate { get; set; }
    public int ViewCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Teaser { get; set; }
    public string SubTitle { get; set; }
    public TextCreateDTO? Text { get; set; }
    public IEnumerable<VideoCreateDTO> Videos { get; set; }
    public IEnumerable<StreetcodeTagDTO> Tags { get; set; }
    public IEnumerable<PartnerShortDTO> Partners { get; set; }
    public IEnumerable<TimelineItemDTO> TimelineItems { get; set; }
    public IEnumerable<StreetcodeDTO> RelatedFigures { get; set; }
  }
}
