using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Newss;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Newss
{
    public class NewsRepository : RepositoryBase<News>, INewsRepository
    {
        public NewsRepository(StreetcodeDbContext dbContext)
        : base(dbContext)
        {
        }
    }
}
