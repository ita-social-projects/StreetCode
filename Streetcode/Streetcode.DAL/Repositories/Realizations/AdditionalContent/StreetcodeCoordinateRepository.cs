using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.AdditionalContent;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.AdditionalContent;

public class StreetcodeCoordinateRepository : RepositoryBase<StreetcodeCoordinate>, IStreetcodeCoordinateRepository
{
    public StreetcodeCoordinateRepository(StreetcodeDbContext dbContext)
        : base(dbContext)
    {
    }
}