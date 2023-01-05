using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Streetcode;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Streetcode;

internal class RelatedFigureRepository : RepositoryBase<RelatedFigure>, IRelatedFigureRepository
{
    public RelatedFigureRepository(StreetcodeDbContext context)
        : base(context)
    {
    }
}