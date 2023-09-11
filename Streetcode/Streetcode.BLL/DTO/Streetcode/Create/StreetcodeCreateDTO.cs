using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.DTO.Toponyms;
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
        public string? Alias { get; set; }
        public string TransliterationUrl { get; set; }
        public string? ARBlockURL { get; set; }
        public StreetcodeType StreetcodeType { get; set; }
        public StreetcodeStatus Status { get; set; }
        public DateTime EventStartOrPersonBirthDate { get; set; }
        public DateTime? EventEndOrPersonDeathDate { get; set; }
        public int ViewCount { get; set; }
        public string Teaser { get; set; }
        public TextCreateDTO? Text { get; set; }
        public int? AudioId { get; set; }
        public IEnumerable<StreetcodeToponymUpdateDTO> Toponyms { get; set; }
        public IEnumerable<int> ImagesIds { get; set; }
        public IEnumerable<StreetcodeTagDTO> Tags { get; set; }
        public IEnumerable<SubtitleCreateDTO> Subtitles { get; set; }
        public IEnumerable<FactUpdateCreateDto> Facts { get; set; }
        public IEnumerable<VideoCreateDTO> Videos { get; set; }
        public IEnumerable<TimelineItemCreateUpdateDTO> TimelineItems { get; set; }
        public IEnumerable<RelatedFigureShortDTO> RelatedFigures { get; set; }
        public IEnumerable<PartnerShortDTO> Partners { get; set; }
        public IEnumerable<StreetcodeArtSlideCreateUpdateDTO> StreetcodeArtSlides { get; set; }
        public IEnumerable<CategoryContentCreateDTO> StreetcodeCategoryContents { get; set; }
        public IEnumerable<StreetcodeCoordinateDTO> Coordinates { get; set; }
        public IEnumerable<StatisticRecordDTO> StatisticRecords { get; set; }
        public IEnumerable<ImageDetailsDto>? ImagesDetails { get; set; }
    }
}
