namespace Repositories.Interfaces;

public interface IImageRepository
{
    public string GetPictureAsync();
    public void UploadPictureAsync();
    public void DeletePictureAsync();

}