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
    public List<ToponymDTO> Toponyms;
    public CoordinatesDTO Coordinate;
    public List<ImageDTO> Images;
    public int Code;
    public DateTime EventStartOrPersonBirthDate;
    public DateTime EventEndOrPersonDeathDate;
    public int ViewCount;
    public DateTime CreatedAt;
    public DateTime UpdatedAt;
    public List<TagDTO> Tags;
    public string Teaser;
    public AudioDTO Audio;
    public TransactLinkDTO TransactLink;
    public string MainText;
    public List<VideoDTO> Videos;
    public List<FactDTO> Facts;
    public List<TimelineDTO> TimelineItems;
    public List<SourceLinkDTO> SourceLinks;
    public List<ArtDTO> Arts;
    public List<SubtitleDTO> Subtitles;
    public List<PartnerDTO> Sponsors;
    public List<PartnerDTO> StreetcodePartners;
    public List<StreetcodeDTO> RelatedFugures;
}