namespace Streetcode.BLL.Interfaces.BlobStorage;

public interface IBlobService
{
    public void SaveFileInStorage(string base64, string name, string mimeType);
    public (string encodedBase, string mimetype) FindFileInStorage(string base64, string name);
}
