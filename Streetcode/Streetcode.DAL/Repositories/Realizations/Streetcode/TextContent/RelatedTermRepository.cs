using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Streetcode.TextContent
{
    public class RelatedTermRepository : RepositoryBase<RelatedTerm>, IRelatedTermRepository
    {
        public RelatedTermRepository(StreetcodeDbContext streetcodeDbContext)
        : base(streetcodeDbContext)
        {
        }
    }
}
