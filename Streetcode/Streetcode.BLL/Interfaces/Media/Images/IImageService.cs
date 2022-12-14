namespace Streetcode.BLL.Interfaces.Media.Images;

public interface IImageService
{
    public void GetPictureAsync();
    public void UploadPictureAsync();
    public void DeletePictureAsync(); 
}
