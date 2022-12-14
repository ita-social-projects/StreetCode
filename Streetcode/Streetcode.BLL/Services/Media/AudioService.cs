using Streetcode.BLL.Interfaces.Media;
using StreetCode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.Media;

public class AudioService: IAudioService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public AudioService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public void GetAudioAsync()
    {
        // TODO implement here
    }

    public void UploadAudioAsync()
    {
        // TODO implement here
    }

    public void DeleteAudioAsync()
    {
        // TODO implement here
    }
}
