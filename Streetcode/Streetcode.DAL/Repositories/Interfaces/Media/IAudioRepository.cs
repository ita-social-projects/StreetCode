
namespace Repositories.Interfaces;

public interface IAudioRepository
{
    public void GetAudioAsync();
    public void UploadAudioAsync();
    public void DeleteAudioAsync();

}