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

    public List<ToponymDTO> Toponym;

    public CoordinatesDTO Coordinates;

    public List<ImageDTO> Images;

    public int Code;

    public DateTime StartDate;

    public DateTime EndDate;

    public int NumberOfViews;

    public DateTime CreateDate;

    public DateTime UpdateDate;

    public List<TagDTO> Tags;

    public string Teaser;

    public AudioDTO Audio;

    public TransactLinkDTO TransactLink;

    public string MainText;

    public VideoDTO Video;

    public List<FactDTO> Facts;

    public TimelineDTO Timeline;

    public List<SourceLinkDTO> SourceLinks;

    public List<ArtDTO> Arts;

    public List<SubtitleDTO> Subtitles;

    public List<PartnerDTO> Sponsors;

    public List<PartnerDTO> Partners;

    public List<StreetcodeDTO> RelatedFugures;

}