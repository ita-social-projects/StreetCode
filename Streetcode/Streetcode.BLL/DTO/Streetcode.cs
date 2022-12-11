
using Models;

namespace DTO
{
    public abstract class Streetcode
    {

        public int Id;

        public HashSet<Toponym> Toponym;

        public Coordinates Coordinates;

        public HashSet<Image> Images;

        public int Code;

        public DateTime StartDate;

        public DateTime EndDate;

        public int NumberOfViews;

        public DateTime CreateDate;

        public DateTime UpdateDate;

        public HashSet<Tag> Tags;

        public string Teaser;

        public Audio Audio;

        public TransactLink TransactLink;

        public string MainText;

        public Video Video;

        public HashSet<Fact> Facts;

        public Timeline Timeline;

        public HashSet<SourceLink> SourceLinks;

        public HashSet<Art> Arts;

        public HashSet<Subtitle> Subtitles;

        public HashSet<Partner> Sponsors;

        public HashSet<Partner> Partners;

        public HashSet<Streetcode> RelatedFugures;

    }
}