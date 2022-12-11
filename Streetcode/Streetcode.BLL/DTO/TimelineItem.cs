
using Models;

namespace DTO
{
    public class TimelineItem 
    {

        public int Id;

        public string Title;

        public string Description;

        public int Date;

        public Image Image;

        public HashSet<HistoricalContext> HistoricalContext;

    }
}