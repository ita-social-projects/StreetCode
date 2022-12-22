using Streetcode.BLL.Interfaces.Media.Images;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.Media.Images;

public class ArtService : IArtService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public ArtService(IRepositoryWrapper repositoryWrapper)
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
