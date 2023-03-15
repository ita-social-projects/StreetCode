using System.Security.Cryptography;
using System.Text;
using Streetcode.BLL.Interfaces.BlobStorage;

namespace Streetcode.BLL.Services.BlobStorage;

public class BlobService : IBlobService
{
    private readonly string _keyCrypt = "SlavaKasterovSuperGoodInshalaKey"; //32 length
    private readonly string _blobPath = "../../BlobStorage/";
    public (string encodedBase, string mimetype) FindFileInStorage(string name)
    {
        string[] splitedName = name.Split('.');

        return DecryptFile(splitedName[0], splitedName[1]);
    }

    public void SaveFileInStorage(string base64, string name, string mimeType)
    {
        byte[] imageBytes = Convert.FromBase64String(base64);
        string fileExtension = "." + mimeType.Split('/')[1];
        string createdFileName = $"{DateTime.Now}{name}"
            .Replace(" ", "_")
            .Replace(".", "_")
            .Replace(":", "_");

        EncryptFile(imageBytes, fileExtension, createdFileName);
    }

    private string HashFunction(string createdFileName)
    {
        using (var hash = SHA256.Create())
        {
            Encoding enc = Encoding.UTF8;
            byte[] result = hash.ComputeHash(enc.GetBytes(createdFileName));
            return Convert.ToBase64String(result);
        }
    }

    private void EncryptFile(byte[] imageBytes, string type, string name)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(_keyCrypt);

        byte[] iv = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(iv);
        }

        byte[] encryptedBytes;
        using (Aes aes = Aes.Create())
        {
            aes.KeySize = 256;
            aes.Key = keyBytes;
            aes.IV = iv;
            ICryptoTransform encryptor = aes.CreateEncryptor();
            encryptedBytes = encryptor.TransformFinalBlock(imageBytes, 0, imageBytes.Length);
        }

        byte[] encryptedData = new byte[encryptedBytes.Length + iv.Length];
        Buffer.BlockCopy(iv, 0, encryptedData, 0, iv.Length);
        Buffer.BlockCopy(encryptedBytes, 0, encryptedData, iv.Length, encryptedBytes.Length);
        File.WriteAllBytes($"{_blobPath}{name}{type}", encryptedData);
    }

    private (string encodedBase, string mimeType) DecryptFile(string fileName, string type)
    {
        byte[] encryptedData = File.ReadAllBytes($"{_blobPath}{fileName}.{type}");
        byte[] keyBytes = Encoding.UTF8.GetBytes(_keyCrypt);

        byte[] iv = new byte[16];
        Buffer.BlockCopy(encryptedData, 0, iv, 0, iv.Length);

        byte[] decryptedBytes;
        using (Aes aes = Aes.Create())
        {
            aes.KeySize = 256;
            aes.Key = keyBytes;
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor();
            decryptedBytes = decryptor.TransformFinalBlock(encryptedData, iv.Length, encryptedData.Length - iv.Length);
        }

        string encodedBase = Convert.ToBase64String(decryptedBytes);

        return (encodedBase, type);
    }
}