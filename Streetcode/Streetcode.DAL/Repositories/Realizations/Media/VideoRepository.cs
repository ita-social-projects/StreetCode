using Repositories.Interfaces;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Entities.Media;

namespace Repositories.Realizations;

public class VideoRepository : RepositoryBase<Video>, IVideoRepository
{
    public VideoRepository(StreetcodeDbContext streetcodeDBContext) : base(streetcodeDBContext)
    {
    }
}
