using Repositories.Interfaces;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Entities.Media.Images;

namespace Repositories.Realizations;

public class ArtRepository : RepositoryBase<Art>, IArtRepository
{ 
    public ArtRepository(StreetcodeDbContext _streetcodeDBContext):base(_streetcodeDBContext)
    {
    }
}
