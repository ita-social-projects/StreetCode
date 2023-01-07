using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Source;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Source;

public class SourceCategoryRepository : RepositoryBase<SourceLinkCategory>, ISourceCategoryRepository
{
    public SourceCategoryRepository(StreetcodeDbContext dbContext)
        : base(dbContext)
    {
    }
}