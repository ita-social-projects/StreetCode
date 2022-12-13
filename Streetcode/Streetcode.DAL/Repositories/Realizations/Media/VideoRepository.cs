using Repositories.Interfaces;
using System.Globalization;
using Streetcode.DAL.Persistence;

using Streetcode.DAL.Entities.Media.Images;

namespace Repositories.Realizations;

public class VideoRepository : RepositoryBase<Art>, IVideoRepository
{

    public VideoRepository(StreetcodeDbContext _streetcodeDBContext)
    {
    }
  
    public void GetVideoAsync()
    {
        // TODO implement here
    }

    public void UploadVideoAsync()
    {
        // TODO implement here
    }

    public void DeleteVideoAsync()
    {
        // TODO implement here
    }

}
