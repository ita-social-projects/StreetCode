using Streetcode.DAL.Entities.Analytics;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Analytics;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Analytics
{
    public class StatisticRecordsRepository : RepositoryBase<StatisticRecord>, IStatisticRecordRepository
    {
        public StatisticRecordsRepository(StreetcodeDbContext context)
            : base(context)
        {
        }
    }
}
