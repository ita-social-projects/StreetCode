using Repositories.Interfaces;
using System.Globalization;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Entities.Media.Images;

namespace Repositories.Realizations;

public class ArtRepository : RepositoryBase<Art>, IArtRepository
{

    public ArtRepository(StreetcodeDbContext _streetcodeDBContext)
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
