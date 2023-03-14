using System.Security.Cryptography;
using System.Text;
using Streetcode.BLL.Interfaces.BlobStorage;

namespace Streetcode.BLL.Services.BlobStorage;

public class BlobService : IBlobService
{
    private readonly string _blobPath = "../../BlobStorage/";
    public (string encodedBase, string mimetype) FindFileInStorage(string name)
    {
        string filePath = $"{_blobPath}{name}";

        string mimeType = name.Split('.')[1];

        var encodedBase = Convert.ToBase64String(File.ReadAllBytes(filePath));

        return (encodedBase, mimeType);
    }

    public void SaveFileInStorage(string base64, string name, string mimeType)
    {
        byte[] imageBytes = Convert.FromBase64String(base64);
        string fileExtension = "." + mimeType.Split('/')[1];
        string createdFileName = $"{DateTime.Now}{name}"
            .Replace(" ", "_")
            .Replace(".", "_")
            .Replace(":", "_");

        EncryptFunction(imageBytes, $"{_blobPath}TEST.bin", "key228");

        DecryptFunction($"{_blobPath}TEST.bin", $"{_blobPath}{createdFileName}{fileExtension}", "key228");
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

    private void EncryptFunction(byte[] inputFile, string outputFile, string keyString)
    {
        // Generate a 256-bit key from the user-supplied key string
        byte[] key = new byte[32];
        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(keyString);
        for (int i = 0; i < keyBytes.Length && i < key.Length; i++)
        {
            key[i] = keyBytes[i];
        }

        // Generate a random initialization vector (IV)
        byte[] iv = new byte[16];
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(iv);
        }

        // Create the AES encryption algorithm
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            // Encrypt the file
            using (var inputFileStream = new MemoryStream(inputFile))
            using (var outputFileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            using (var cryptoStream = new CryptoStream(outputFileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                // Write the IV to the output file
                outputFileStream.Write(iv, 0, iv.Length);

                // Encrypt the file contents and write to the output file
                byte[] buffer = new byte[1024];
                int bytesRead;
                while ((bytesRead = inputFileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    cryptoStream.Write(buffer, 0, bytesRead);
                }
            }
        }
    }

    private void DecryptFunction(string inputFile, string outputFile, string keyString)
    {
        byte[] key = new byte[32];
        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(keyString);
        for (int i = 0; i < keyBytes.Length && i < key.Length; i++)
        {
            key[i] = keyBytes[i];
        }

        // Read the IV from the input file
        byte[] iv = new byte[16];
        using (var inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
        {
            inputFileStream.Read(iv, 0, iv.Length);
        }

        // Create the AES decryption algorithm
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            // Decrypt the file
            using (FileStream inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            using (FileStream outputFileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            using (CryptoStream cryptoStream = new CryptoStream(inputFileStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
            {
                // Decrypt the file contents and write to the output file
                byte[] buffer = new byte[1024];
                int bytesRead;
                while ((bytesRead = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    outputFileStream.Write(buffer, 0, bytesRead);
                }
            }
        }
    }
}