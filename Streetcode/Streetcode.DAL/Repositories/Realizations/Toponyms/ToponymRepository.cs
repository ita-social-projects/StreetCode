using Repositories.Realizations;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Toponyms;

namespace Streetcode.DAL.Repositories.Realizations.Toponyms;

public class ToponymRepository : RepositoryBase<Toponym>, IToponymRepository
{
    public ToponymRepository(StreetcodeDbContext _streetcodeDBContext):base(_streetcodeDBContext)
    {
    }
}