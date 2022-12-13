namespace Repositories.Interfaces;

public interface IVideoRepository
{

    public void GetVideoAsync();
    public void UploadVideoAsync();
    public void DeleteVideoAsync();

}