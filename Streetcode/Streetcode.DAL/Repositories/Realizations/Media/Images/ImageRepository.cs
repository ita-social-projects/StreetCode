using Repositories.Interfaces;
using System.Globalization;

using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Persistence;

namespace Repositories.Realizations;

public class ImageRepository : RepositoryBase<Image>, IImageRepository
{

    public ImageRepository(StreetcodeDbContext _streetcodeDBContext)
    {
    }

    public string GetPictureAsync()
    {
        return "GetPictureAsync";
        // TODO implement here
    }

    public void UploadPictureAsync()
    {
        // TODO implement here
    }

    public void DeletePictureAsync()
    {
        // TODO implement here
    }

}