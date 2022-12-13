
namespace DTO;

public abstract class StreetcodeDTO
{

    public int Id;

    public HashSet<ToponymDTO> Toponym;

    public CoordinatesDTO Coordinates;

    public HashSet<ImageDTO> Images;

    public int Code;

    public DateTime StartDate;

    public DateTime EndDate;

    public int NumberOfViews;

    public DateTime CreateDate;

    public DateTime UpdateDate;

    public HashSet<TagDTO> Tags;

    public string Teaser;

    public AudioDTO Audio;

    public TransactLinkDTO TransactLink;

    public string MainText;

    public VideoDTO Video;

    public HashSet<FactDTO> Facts;

    public TimelineDTO Timeline;

    public HashSet<SourceLinkDTO> SourceLinks;

    public HashSet<ArtDTO> Arts;

    public HashSet<SubtitleDTO> Subtitles;

    public HashSet<PartnerDTO> Sponsors;

    public HashSet<PartnerDTO> Partners;

    public HashSet<StreetcodeDTO> RelatedFugures;

}