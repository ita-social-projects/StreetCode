using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Media.Images;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Media.Images;

public class StreetcodeArtRepository : RepositoryBase<StreetcodeArt>, IStreetcodeArtRepository
{
    public StreetcodeArtRepository(StreetcodeDbContext dbContext)
        : base(dbContext)
    {
    }
}
