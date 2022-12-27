using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Streetcode;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Realizations.Base;
using Streetcode.DAL.Entities.Streetcode.Types;

namespace Streetcode.DAL.Repositories.Realizations.Streetcode;

public class EventStreetcodeRepository : RepositoryBase<EventStreetcode>, IEventStreetcodeRepository
{
    public EventStreetcodeRepository(StreetcodeDbContext dbContext)
        : base(dbContext)
    {
    }
}
