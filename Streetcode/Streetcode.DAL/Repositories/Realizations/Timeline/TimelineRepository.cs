using Repositories.Realizations;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Timeline;

namespace Streetcode.DAL.Repositories.Realizations.Timeline;

public class TimelineRepository : RepositoryBase<TimelineItem>, ITimelineRepository
{

    public TimelineRepository(StreetcodeDbContext _streetcodeDBContext):base(_streetcodeDBContext)
    {
    }
}