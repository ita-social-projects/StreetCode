using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Partners;
using Streetcode.DAL.Repositories.Interfaces.Source;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Source;

public class SourceLinkRepository : RepositoryBase<SourceLink>, ISourceLinkRepository
{
    public SourceLinkRepository(StreetcodeDbContext dbContext)
        : base(dbContext)
    {
    }
}
