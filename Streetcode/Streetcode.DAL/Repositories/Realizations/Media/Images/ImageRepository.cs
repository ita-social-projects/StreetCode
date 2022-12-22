using Repositories.Interfaces;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Persistence;

namespace Repositories.Realizations;

public class ImageRepository : RepositoryBase<Image>, IImageRepository
{
    public ImageRepository(StreetcodeDbContext streetcodeDbContext)
        : base(streetcodeDbContext)
    {
    }
}