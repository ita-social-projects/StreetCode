namespace Streetcode.BLL.Interfaces.Image;

public interface IImageService
{
    public Task CleanUnusedImagesAsync();
}