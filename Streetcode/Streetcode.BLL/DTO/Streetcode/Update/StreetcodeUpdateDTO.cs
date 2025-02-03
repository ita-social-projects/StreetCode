using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Analytics.Update;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.DTO.Partners.Update;
using Streetcode.BLL.DTO.Sources.Update;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Streetcode.Update
{
    public class StreetcodeUpdateDto : StreetcodeCreateUpdateDto
    {
        public int Id { get; set; }
        public TextUpdateDto? Text { get; set; }
        public string? ARBlockUrl { get; set; }
        public IEnumerable<SubtitleUpdateDto> Subtitles { get; set; } = null!;
        public IEnumerable<StreetcodeFactUpdateDto> Facts { get; set; }
        public IEnumerable<VideoUpdateDto>? Videos { get; set; }
        public IEnumerable<AudioUpdateDto> Audios { get; set; } = null!;
        public IEnumerable<RelatedFigureUpdateDto> RelatedFigures { get; set; } = null!;
        public IEnumerable<PartnersUpdateDto> Partners { get; set; } = null!;
        public IEnumerable<StreetcodeTagUpdateDto> Tags { get; set; } = null!;
        public IEnumerable<StatisticRecordUpdateDto> StatisticRecords { get; set; }
        public IEnumerable<ImageUpdateDto> Images { get; set; } = null!;
        public IEnumerable<StreetcodeCategoryContentUpdateDto> StreetcodeCategoryContents { get; set; }
    }
}
