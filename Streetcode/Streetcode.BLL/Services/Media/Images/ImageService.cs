using Streetcode.BLL.Interfaces.Media.Images;
using StreetCode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.Media.Images;

public class ImageService: IImageService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public ImageService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public void GetPictureAsync()
    {
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
