using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;

namespace Streetcode.BLL.DTO.Streetcode.Create
{
  public class StreetcodeCreateDto : StreetcodeCreateUpdateDto
    {
        public string? ARBlockURL { get; set; }
        public int ViewCount { get; set; }
        public TextCreateDto? Text { get; set; }
        public int? AudioId { get; set; }
        public IEnumerable<int> ImagesIds { get; set; } = null!; // one image black and white is required at front-end side, so at least one will be passed
        public IEnumerable<StreetcodeTagDto> Tags { get; set; } = null!;
        public IEnumerable<SubtitleCreateDto> Subtitles { get; set; } = null!; // subtitles are only in one example
        public IEnumerable<StreetcodeFactCreateDto> Facts { get; set; } = null!;
        public IEnumerable<VideoCreateDto>? Videos { get; set; } = null!; // video is only one
        public IEnumerable<RelatedFigureShortDto> RelatedFigures { get; set; } = null!;
        public IEnumerable<int> Partners { get; set; } = null!;
        public IEnumerable<CategoryContentCreateDto> StreetcodeCategoryContents { get; set; } = null!;
        public IEnumerable<StreetcodeCoordinateDto> Coordinates { get; set; } = null!;
        public IEnumerable<StatisticRecordDto> StatisticRecords { get; set; } = null!;
    }
}
