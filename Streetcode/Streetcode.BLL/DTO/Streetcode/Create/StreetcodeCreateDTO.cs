using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;

namespace Streetcode.BLL.DTO.Streetcode.Create;

public class StreetcodeCreateDTO : StreetcodeCreateUpdateDTO
{
    public string? ArBlockUrl { get; set; }

    public int ViewCount { get; set; }

    public TextCreateDTO? Text { get; set; }

    public int? AudioId { get; set; }

    public IEnumerable<StreetcodeTagDTO>? Tags { get; set; } = new List<StreetcodeTagDTO>();

    public IEnumerable<SubtitleCreateDTO>? Subtitles { get; set; } = new List<SubtitleCreateDTO>();

    public IEnumerable<StreetcodeFactCreateDTO>? Facts { get; set; } = new List<StreetcodeFactCreateDTO>();

    public IEnumerable<VideoCreateDTO>? Videos { get; set; } = new List<VideoCreateDTO>();

    public IEnumerable<RelatedFigureShortDTO>? RelatedFigures { get; set; } = new List<RelatedFigureShortDTO>();

    public IEnumerable<int>? Partners { get; set; } = new List<int>();

    public IEnumerable<CategoryContentCreateDTO>? StreetcodeCategoryContents { get; set; } = new List<CategoryContentCreateDTO>();

    public IEnumerable<StreetcodeCoordinateDTO>? Coordinates { get; set; } = new List<StreetcodeCoordinateDTO>();

    public IEnumerable<StatisticRecordDTO>? StatisticRecords { get; set; } = new List<StatisticRecordDTO>();
}