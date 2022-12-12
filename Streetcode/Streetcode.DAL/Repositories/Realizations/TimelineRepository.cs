using Repositories.Interfaces;
using Streetcode.DAL.Persistence;


namespace Repositories.Realizations;

public class TimelineRepository : RepositoryBase , ITimelineRepository 
{

    public TimelineRepository(StreetcodeDbContext _streetcodeDBContext) 
    {
    }

    public string GetTimeItemsByStreetcode()
    {
        return "GetTimeItemsByStreetcode";
        // TODO implement here
    }

}