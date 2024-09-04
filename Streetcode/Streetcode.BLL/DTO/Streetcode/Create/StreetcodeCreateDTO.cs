using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.DTO.ArtGallery.ArtSlide;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Streetcode.Create
{
  public class StreetcodeCreateDTO
    {
        public int Index { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Title { get; set; } = null!;
        public string DateString { get; set; } = null!;
        public string? Alias { get; set; }
        public string TransliterationUrl { get; set; } = null!;
        public string? ARBlockURL { get; set; }
        public StreetcodeType StreetcodeType { get; set; }
        public StreetcodeStatus Status { get; set; }
        public DateTime EventStartOrPersonBirthDate { get; set; }
        public DateTime? EventEndOrPersonDeathDate { get; set; }
        public int ViewCount { get; set; }
        public string Teaser { get; set; } = null!;
        public TextCreateDTO? Text { get; set; }
        public int? AudioId { get; set; }
        public IEnumerable<StreetcodeToponymUpdateDTO> Toponyms { get; set; } = new List<StreetcodeToponymUpdateDTO>();
        public IEnumerable<int> ImagesIds { get; set; } = new List<int>();
        public IEnumerable<StreetcodeTagDTO> Tags { get; set; } = new List<StreetcodeTagDTO>();
        public IEnumerable<SubtitleCreateDTO> Subtitles { get; set; } = new List<SubtitleCreateDTO>();
        public IEnumerable<FactUpdateCreateDto> Facts { get; set; } = new List<FactUpdateCreateDto>();
        public IEnumerable<VideoCreateDTO> Videos { get; set; } = new List<VideoCreateDTO>();
        public IEnumerable<TimelineItemCreateUpdateDTO> TimelineItems { get; set; } = new List<TimelineItemCreateUpdateDTO>();
        public IEnumerable<RelatedFigureShortDTO> RelatedFigures { get; set; } = new List<RelatedFigureShortDTO>();
        public IEnumerable<PartnerShortDTO> Partners { get; set; } = new List<PartnerShortDTO>();
        public IEnumerable<StreetcodeArtSlideCreateUpdateDTO> StreetcodeArtSlides { get; set; } = new List<StreetcodeArtSlideCreateUpdateDTO>();
        public IEnumerable<CategoryContentCreateDTO> StreetcodeCategoryContents { get; set; } = new List<CategoryContentCreateDTO>();
        public IEnumerable<StreetcodeCoordinateDTO> Coordinates { get; set; } = new List<StreetcodeCoordinateDTO>();
        public IEnumerable<StatisticRecordDTO> StatisticRecords { get; set; } = new List<StatisticRecordDTO>();
        public IEnumerable<ImageDetailsDto> ImagesDetails { get; set; } = new List<ImageDetailsDto>();
        public List<ArtCreateUpdateDTO> Arts { get; set; } = new List<ArtCreateUpdateDTO>();
    }
}
