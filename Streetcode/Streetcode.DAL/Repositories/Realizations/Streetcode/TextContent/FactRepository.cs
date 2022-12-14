using Repositories.Realizations;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Streetcode.TextContent;

namespace Streetcode.DAL.Repositories.Realizations.Streetcode.TextContent;

public class FactRepository : RepositoryBase<Fact>, IFactRepository
{

    public FactRepository(StreetcodeDbContext _streetcodeDbContext):base(_streetcodeDbContext)
    {
    }
}