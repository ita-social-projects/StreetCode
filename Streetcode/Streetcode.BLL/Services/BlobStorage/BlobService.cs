using Streetcode.BLL.Interfaces.BlobStorage;

namespace Streetcode.BLL.Services.BlobStorage;

public class BlobService : IBlobService
{
    public (string encodedBase, string mimetype) FindFileInStorage(string base64, string name)
    {
        string filePath = $"../../BlobStorage/{name}";

        string mimeType = name.Split('.')[1];

        var encodedBase = Convert.ToBase64String(File.ReadAllBytes(filePath));

        return (encodedBase, mimeType);
    }

    public void SaveFileInStorage(string base64, string name, string mimeType)
    {
        byte[] imageBytes = Convert.FromBase64String(base64);

        string fileExtension = "." + mimeType.Split('/')[1];

        string fileName = name + fileExtension;

        File.WriteAllBytes($"./StreetCode/BlobStorage/{fileName}", imageBytes);
    }
}