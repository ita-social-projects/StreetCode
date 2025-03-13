using Streetcode.DAL.Entities.Event;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Event;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Event
{
    internal class EventStreetcodesRepository : RepositoryBase<EventStreetcodes>, IEventStreetcodesRepository
    {
        public EventStreetcodesRepository(StreetcodeDbContext context)
            : base(context)
        {
        }
    }
}
