
using Repositories.Interfaces;


namespace Repositories.Realizations
{
    public class TimelineRepository : RepositoryBase , ITimelineRepository 
    {

        public TimelineRepository(StreetcodeDBContext _streetcodeDBContext) 
        {
        }

        public void GetTimeItemsByStreetcode()
        {
            // TODO implement here
        }

    }
}