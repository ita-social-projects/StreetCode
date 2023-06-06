using Streetcode.DAL.Enums;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.DTO.Partners.Update;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;

namespace Streetcode.BLL.DTO.Streetcode.Update
{
  public class StreetcodeUpdateDTO
  {
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int Index { get; set; }
    public string? Teaser { get; set; }
    public string DateString { get; set; }
    public string? Alias { get; set; }
    public StreetcodeStatus Status { get; set; }
    public string Title { get; set; }
    public string TransliterationUrl { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime EventStartOrPersonBirthDate { get; set; }
    public DateTime? EventEndOrPersonDeathDate { get; set; }
    public TextUpdateDTO? Text { get; set; }
    /*public IEnumerable<StreetcodeToponymUpdateDTO> StreetcodeToponym { get; set; }*/
    public IEnumerable<SubtitleUpdateDTO> Subtitles { get; set; }
    public IEnumerable<FactUpdateDTO> Facts { get; set; }
    public IEnumerable<VideoUpdateDTO> Videos { get; set; }
    public IEnumerable<RelatedFigureUpdateDTO> RelatedFigures { get; set; }
    public IEnumerable<PartnersUpdateDTO> Partners { get; set; }
    public IEnumerable<TimelineItemUpdateDTO> TimelineItems { get; set; }
    /*public IEnumerable<StreetcodeArtUpdateDTO> StreetcodeArts { get; set; }*/
  }
}
