using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Source;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Source;

public class SourceSubCategoryRepository : RepositoryBase<StreetcodeCategoryContent>, ISourceSubCategoryRepository
{
    public SourceSubCategoryRepository(StreetcodeDbContext dbContext)
        : base(dbContext)
    {
    }
}