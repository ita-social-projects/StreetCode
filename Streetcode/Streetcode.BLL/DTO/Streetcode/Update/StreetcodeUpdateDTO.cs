using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Analytics.Update;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.DTO.Partners.Update;
using Streetcode.BLL.DTO.Sources.Update;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;

namespace Streetcode.BLL.DTO.Streetcode.Update;

public class StreetcodeUpdateDTO : StreetcodeCreateUpdateDTO
{
    public int Id { get; set; }

    public TextUpdateDTO? Text { get; set; }

    public string? ArBlockUrl { get; set; }

    public IEnumerable<SubtitleUpdateDTO>? Subtitles { get; set; } = new List<SubtitleUpdateDTO>();

    public IEnumerable<StreetcodeFactUpdateDTO>? Facts { get; set; } = new List<StreetcodeFactUpdateDTO>();

    public IEnumerable<VideoUpdateDTO>? Videos { get; set; } = new List<VideoUpdateDTO>();

    public IEnumerable<AudioUpdateDTO>? Audios { get; set; } = new List<AudioUpdateDTO>();

    public IEnumerable<RelatedFigureUpdateDTO>? RelatedFigures { get; set; } = new List<RelatedFigureUpdateDTO>();

    public IEnumerable<PartnersUpdateDTO>? Partners { get; set; } = new List<PartnersUpdateDTO>();

    public IEnumerable<StreetcodeTagUpdateDTO>? Tags { get; set; } = new List<StreetcodeTagUpdateDTO>();

    public IEnumerable<StatisticRecordUpdateDTO>? StatisticRecords { get; set; } = new List<StatisticRecordUpdateDTO>();

    public IEnumerable<ImageUpdateDTO> Images { get; set; } = new List<ImageUpdateDTO>();

    public IEnumerable<StreetcodeCategoryContentUpdateDTO>? StreetcodeCategoryContents { get; set; } = new List<StreetcodeCategoryContentUpdateDTO>();
}