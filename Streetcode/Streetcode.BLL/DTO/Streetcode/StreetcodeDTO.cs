using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.DTO.Transactions;

namespace Streetcode.BLL.DTO.Streetcode;

public abstract class StreetcodeDTO
{
    public int Id { get; set; }
    public int Index { get; set; }
    public IEnumerable<ToponymDTO> Toponyms { get; set; }
    public CoordinateDTO Coordinate { get; set; }
    public IEnumerable<ImageDTO> Images { get; set; }
    public DateTime EventStartOrPersonBirthDate { get; set; }
    public DateTime EventEndOrPersonDeathDate { get; set; }
    public int ViewCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public IEnumerable<TagDTO> Tags { get; set; }
    public string Teaser { get; set; }
    public AudioDTO Audio { get; set; }
    public TransactLinkDTO TransactionLink { get; set; }
    public string Text { get; set; }
    public IEnumerable<VideoDTO> Videos { get; set; }
    public IEnumerable<FactDTO> Facts { get; set; }
    public IEnumerable<TimelineItemDTO> TimelineItems { get; set; }
    public IEnumerable<SourceLinkDTO> SourceLinks { get; set; }
    public IEnumerable<ArtDTO> Arts { get; set; }
    public IEnumerable<SubtitleDTO> Subtitles { get; set; }
    public IEnumerable<StreetcodePartnerDTO> StreetcodePartners { get; set; }
    public IEnumerable<RelatedFigureDTO> Observers { get; set; }
    public IEnumerable<RelatedFigureDTO> Targets { get; set; }
}