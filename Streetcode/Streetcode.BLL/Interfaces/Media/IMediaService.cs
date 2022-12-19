namespace Streetcode.BLL.Interfaces.Media;

public interface IMediaService
{
    public void GetPictureAsync();
    public void UploadPictureAsync();
    public void DeletePictureAsync();
    public void GetVideoAsync();
    public void UploadVideoAsync();
    public void DeleteVideoAsync();
    public void GetAudioAsync();
    public void UploadAudioAsync();
    public void DeleteAudioAsync();
}