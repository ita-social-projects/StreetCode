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
    public class StreetcodeUpdateDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Index { get; set; }
        public string? Teaser { get; set; }
        public string DateString { get; set; } = null!;
        public string? Alias { get; set; }
        public StreetcodeStatus Status { get; set; }
        public StreetcodeType StreetcodeType { get; set; }
        public string Title { get; set; } = null!;
        public string TransliterationUrl { get; set; } = null!;
        public DateTime EventStartOrPersonBirthDate { get; set; }
        public DateTime? EventEndOrPersonDeathDate { get; set; }
        public TextUpdateDTO? Text { get; set; }
        public TransactionLinkUpdateDTO TransactionLink { get; set; } = null!;
        public IEnumerable<StreetcodeToponymUpdateDTO> Toponyms { get; set; } = new List<StreetcodeToponymUpdateDTO>();
        public IEnumerable<SubtitleUpdateDTO> Subtitles { get; set; } = new List<SubtitleUpdateDTO>();
        public IEnumerable<FactUpdateDto> Facts { get; set; } = new List<FactUpdateDto>();
        public IEnumerable<VideoUpdateDTO> Videos { get; set; } = new List<VideoUpdateDTO>();
        public IEnumerable<AudioUpdateDTO> Audios { get; set; } = new List<AudioUpdateDTO>();
        public IEnumerable<RelatedFigureUpdateDTO> RelatedFigures { get; set; } = new List<RelatedFigureUpdateDTO>();
        public IEnumerable<PartnersUpdateDTO> Partners { get; set; } = new List<PartnersUpdateDTO>();
        public IEnumerable<TimelineItemCreateUpdateDTO> TimelineItems { get; set; } = new List<TimelineItemCreateUpdateDTO>();
        public IEnumerable<StreetcodeArtSlideCreateUpdateDTO> StreetcodeArtSlides { get; set; } = new List<StreetcodeArtSlideCreateUpdateDTO>();
        public IEnumerable<ArtCreateUpdateDTO> Arts { get; set; } = new List<ArtCreateUpdateDTO>();
        public IEnumerable<StreetcodeTagUpdateDTO> Tags { get; set; } = new List<StreetcodeTagUpdateDTO>();
        public IEnumerable<StatisticRecordUpdateDTO> StatisticRecords { get; set; } = new List<StatisticRecordUpdateDTO>();
        public IEnumerable<ImageUpdateDTO> Images { get; set; } = new List<ImageUpdateDTO>();
        public IEnumerable<StreetcodeCategoryContentUpdateDTO> StreetcodeCategoryContents { get; set; } = new List<StreetcodeCategoryContentUpdateDTO>();
        public IEnumerable<ImageDetailsDto> ImagesDetails { get; set; } = new List<ImageDetailsDto>();
    }
}
