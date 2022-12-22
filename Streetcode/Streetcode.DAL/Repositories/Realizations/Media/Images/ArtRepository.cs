using Repositories.Interfaces;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Entities.Media.Images;

namespace Repositories.Realizations;

public class ArtRepository : RepositoryBase<Art>, IArtRepository
{
    public ArtRepository(StreetcodeDbContext streetcodeDbContext)
        : base(streetcodeDbContext)
    {
    }
}
