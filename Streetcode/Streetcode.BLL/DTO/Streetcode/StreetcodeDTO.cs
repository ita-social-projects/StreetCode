using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates;
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
    public int Id;
    public int Index;
    public IEnumerable<ToponymDTO> Toponyms;
    public CoordinatesDTO Coordinate;
    public IEnumerable<ImageDTO> Images;
    public int Code;
    public DateTime EventStartOrPersonBirthDate;
    public DateTime EventEndOrPersonDeathDate;
    public int ViewCount;
    public DateTime CreatedAt;
    public DateTime UpdatedAt;
    public IEnumerable<TagDTO> Tags;
    public string Teaser;
    public AudioDTO Audio;
    public TransactLinkDTO TransactLink;
    public string MainText;
    public IEnumerable<VideoDTO> Videos;
    public IEnumerable<FactDTO> Facts;
    public IEnumerable<TimelineDTO> TimelineItems;
    public IEnumerable<SourceLinkDTO> SourceLinks;
    public IEnumerable<ArtDTO> Arts;
    public IEnumerable<SubtitleDTO> Subtitles;
    public IEnumerable<PartnerDTO> Sponsors;
    public IEnumerable<PartnerDTO> StreetcodePartners;
    public IEnumerable<StreetcodeDTO> RelatedFugures;
}