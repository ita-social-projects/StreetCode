using System.Buffers;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Streetcode.BLL.Interfaces.BlobStorage;

namespace Streetcode.BLL.Services.BlobStorageService;

public class BlobService : IBlobService
{
    private readonly BlobEnvironmentVariables _envirovment;
    private readonly string _keyCrypt;
    private readonly string _blobPath;

    public BlobService(IOptions<BlobEnvironmentVariables> environment)
    {
        _envirovment = environment.Value;
        _keyCrypt = _envirovment.BlobStoreKey;
        _blobPath = _envirovment.BlobStorePath;
    }

    public MemoryStream FindFileInStorageAsMemoryStream(string? name)
    {
        ArgumentNullException.ThrowIfNull(name);

        string[] splitedName = name.Split('.');

        byte[] decodedBytes = DecryptFile(splitedName[0], splitedName[1]);

        var image = new MemoryStream(decodedBytes);

        return image;
    }

    public string FindFileInStorageAsBase64(string name)
    {
        using var stream = FindFileInStorageAsMemoryStream(name);
        return Convert.ToBase64String(stream.ToArray());
    }

    public string SaveFileInStorage(string base64, string name, string extension)
    {
        byte[] imageBytes = ConvertBase64ToBytes(base64);
        string createdFileName = $"{DateTime.Now}{name}"
            .Replace(" ", "_")
            .Replace(".", "_")
            .Replace(":", "_");

        string hashBlobStorageName = HashFunction(createdFileName);

        Directory.CreateDirectory(_blobPath);
        EncryptFile(imageBytes, extension, hashBlobStorageName);

        return hashBlobStorageName;
    }

    public void SaveFileInStorageBase64(string base64, string name, string extension)
    {
        byte[] imageBytes = ConvertBase64ToBytes(base64.Trim());
        Directory.CreateDirectory(_blobPath);
        EncryptFile(imageBytes, extension, name);
    }

    public void DeleteFileInStorage(string? name)
    {
        ArgumentNullException.ThrowIfNull(name);

        File.Delete($"{_blobPath}{name}");
    }

    public string UpdateFileInStorage(
        string? previousBlobName,
        string base64Format,
        string newBlobName,
        string extension)
    {
        ArgumentNullException.ThrowIfNull(previousBlobName);

        DeleteFileInStorage(previousBlobName);

        string hashBlobStorageName = SaveFileInStorage(
        base64Format,
        newBlobName,
        extension);

        return hashBlobStorageName;
    }

    private byte[] ConvertBase64ToBytes(string base64)
    {
        int byteCount = (base64.Length * 3) / 4;
        byte[] buffer = ArrayPool<byte>.Shared.Rent(byteCount);
        try
        {
            if (!Convert.TryFromBase64String(base64, buffer, out int bytesWritten))
            {
                throw new FormatException("Invalid Base64 string.");
            }

            return buffer[..bytesWritten];
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    private string HashFunction(string createdFileName)
    {
        using var hash = SHA256.Create();
        byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes(createdFileName));
        return BitConverter.ToString(result).Replace("-", "").ToLower();
    }

    private void EncryptFile(byte[] imageBytes, string type, string name)
    {
        string filePath = Path.Combine(_blobPath, $"{name}.{type}");
        byte[] keyBytes = Encoding.UTF8.GetBytes(_keyCrypt);

        using var aes = Aes.Create();
        using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

        aes.KeySize = 256;
        aes.Key = keyBytes;
        aes.GenerateIV();
        fileStream.Write(aes.IV, 0, aes.IV.Length);

        byte[] buffer = ArrayPool<byte>.Shared.Rent(imageBytes.Length);
        try
        {
            Buffer.BlockCopy(imageBytes, 0, buffer, 0, imageBytes.Length);

            using var cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(buffer, 0, imageBytes.Length);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    private byte[] DecryptFile(string fileName, string type)
    {
        string filePath = Path.Combine(_blobPath, $"{fileName}.{type}");
        using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        using var aes = Aes.Create();

        byte[] keyBytes = Encoding.UTF8.GetBytes(_keyCrypt);
        byte[] iv = new byte[16];

        if (fileStream.Read(iv, 0, iv.Length) != iv.Length)
        {
            throw new IOException("Invalid IV length");
        }

        aes.KeySize = 256;
        aes.Key = keyBytes;
        aes.IV = iv;

        byte[] buffer = ArrayPool<byte>.Shared.Rent((int)(fileStream.Length - iv.Length));

        try
        {
            using var cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            int bytesRead = cryptoStream.Read(buffer, 0, buffer.Length);
            return buffer[..bytesRead];
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
}