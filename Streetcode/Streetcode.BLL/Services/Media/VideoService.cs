using Streetcode.BLL.Interfaces.Media;
using StreetCode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.Media;

public class VideoService: IVideoService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public VideoService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
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
