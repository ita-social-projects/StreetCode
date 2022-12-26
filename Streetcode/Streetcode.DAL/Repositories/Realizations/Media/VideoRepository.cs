using Repositories.Interfaces;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Media;

public class VideoRepository : RepositoryBase<Video>, IVideoRepository
{
    public VideoRepository(StreetcodeDbContext dbContext)
        : base(dbContext)
    {
    }
}
