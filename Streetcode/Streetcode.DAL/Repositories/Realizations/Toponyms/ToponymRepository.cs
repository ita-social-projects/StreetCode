using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Toponyms;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Toponyms;

public class ToponymRepository : RepositoryBase<Toponym>, IToponymRepository
{
    public ToponymRepository(StreetcodeDbContext dbContext)
        : base(dbContext)
    {
    }
}