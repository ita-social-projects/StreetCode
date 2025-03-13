using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.DAL.Repositories.Interfaces.Event
{
    public interface IEventRepository : IRepositoryBase<DAL.Entities.Event.Event>
    {
    }
}
