using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Analytics.Update;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Audio;
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
        public string DateString { get; set; }
        public string? Alias { get; set; }
        public StreetcodeStatus Status { get; set; }
        public StreetcodeType StreetcodeType { get; set; }
        public string Title { get; set; }
        public string TransliterationUrl { get; set; }
        public DateTime EventStartOrPersonBirthDate { get; set; }
        public DateTime? EventEndOrPersonDeathDate { get; set; }
        public TextUpdateDTO? Text { get; set; }
        public TransactionLinkUpdateDTO? TransactionLink { get; set; }
        public IEnumerable<StreetcodeToponymUpdateDTO> Toponyms { get; set; }
        public IEnumerable<SubtitleUpdateDTO> Subtitles { get; set; }
        public IEnumerable<FactUpdateDto> Facts { get; set; }
        public IEnumerable<VideoUpdateDTO> Videos { get; set; }
        public IEnumerable<AudioUpdateDTO> Audios { get; set; }
        public IEnumerable<RelatedFigureUpdateDTO> RelatedFigures { get; set; }
        public IEnumerable<PartnersUpdateDTO> Partners { get; set; }
        public IEnumerable<TimelineItemCreateUpdateDTO> TimelineItems { get; set; }
        public IEnumerable<StreetcodeArtCreateUpdateDTO> StreetcodeArts { get; set; }
        public IEnumerable<StreetcodeTagUpdateDTO> Tags { get; set; }
        public IEnumerable<StatisticRecordUpdateDTO> StatisticRecords { get; set; }
        public IEnumerable<ImageUpdateDTO> Images { get; set; }
        public IEnumerable<StreetcodeCategoryContentUpdateDTO> StreetcodeCategoryContents { get; set; }
        public IEnumerable<ImageDetailsDto>? ImagesDetails { get; set; }
    }
}
