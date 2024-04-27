using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Media.Images;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Media.Images;

public class StreetcodeArtSlideRepository : RepositoryBase<StreetcodeArtSlide>, IStreetcodeArtSlideRepository
{
    public StreetcodeArtSlideRepository(StreetcodeDbContext dbContext)
        : base(dbContext)
    {
    }
}
