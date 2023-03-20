namespace Streetcode.BLL.Interfaces.BlobStorage;

public interface IBlobService
{
    public string SaveFileInStorage(string base64, string name, string mimeType);
    public MemoryStream FindFileInStorage(string name);
}
