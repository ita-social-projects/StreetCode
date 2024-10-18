using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.News;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.News
{
    public class NewsRepository : RepositoryBase<Entities.News.News>, INewsRepository
    {
        public NewsRepository(StreetcodeDbContext dbContext)
        : base(dbContext)
        {
        }
    }
}
