using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Timeline;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Timeline
{
    public class HistoricalContextTimelineRepository : RepositoryBase<HistoricalContextTimeline>, IHistoricalContextTimelineRepository
    {
        public HistoricalContextTimelineRepository(StreetcodeDbContext dbContext)
        : base(dbContext)
        {
        }
    }
}
