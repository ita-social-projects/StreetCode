using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Event;
using Streetcode.DAL.Repositories.Interfaces.Team;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Event
{
    public class EventRepository : RepositoryBase<DAL.Entities.Event.Event>, IEventRepository
    {
        public EventRepository(StreetcodeDbContext context)
            : base(context)
        {
        }
    }
}
