using Repositories.Realizations;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Streetcode;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Repositories.Realizations.Streetcode;

public class StreetcodeRepository : RepositoryBase<StreetcodeContent>, IStreetcodeRepository
{
    public StreetcodeRepository(StreetcodeDbContext _streetcodeDBContext):base(_streetcodeDBContext)
    {
    }
}
