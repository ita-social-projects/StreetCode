
using EFTask.Persistence;
using Repositories.Interfaces;


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