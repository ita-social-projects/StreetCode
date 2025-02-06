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
    public class StreetcodeUpdateDTO : StreetcodeCreateUpdateDTO
    {
        public int Id { get; set; }
        public TextUpdateDTO? Text { get; set; }
        public string? ARBlockUrl { get; set; }
        public IEnumerable<SubtitleUpdateDTO> Subtitles { get; set; } = null!;
        public IEnumerable<StreetcodeFactUpdateDTO> Facts { get; set; }
        public IEnumerable<VideoUpdateDTO>? Videos { get; set; }
        public IEnumerable<AudioUpdateDTO> Audios { get; set; } = new List<AudioUpdateDTO>();
        public IEnumerable<RelatedFigureUpdateDTO> RelatedFigures { get; set; } = null!;
        public IEnumerable<PartnersUpdateDTO> Partners { get; set; } = null!;
        public IEnumerable<StreetcodeTagUpdateDTO> Tags { get; set; } = null!;
        public IEnumerable<StatisticRecordUpdateDTO> StatisticRecords { get; set; } = new List<StatisticRecordUpdateDTO>();
        public IEnumerable<ImageUpdateDTO> Images { get; set; } = new List<ImageUpdateDTO>();
        public IEnumerable<StreetcodeCategoryContentUpdateDTO> StreetcodeCategoryContents { get; set; }
    }
}
