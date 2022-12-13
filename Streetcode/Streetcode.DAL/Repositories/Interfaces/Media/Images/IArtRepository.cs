namespace Repositories.Interfaces;

public interface IArtRepository
{
    public string GetPictureAsync();
    public void UploadPictureAsync();
    public void DeletePictureAsync();

}