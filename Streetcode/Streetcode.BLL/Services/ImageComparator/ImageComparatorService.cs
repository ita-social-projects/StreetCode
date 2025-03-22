using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Streetcode.BLL.Interfaces.ImageComparator;

namespace Streetcode.BLL.Services.ImageComparator;

public class ImageComparatorService : IImageComparatorService
{
    public bool CompareImages(string? img1Base64, string? img2Base64, int acceptedHammingDistance = 10)
    {
        ulong hash1 = GetImageHash(img1Base64);
        ulong hash2 = GetImageHash(img2Base64);
        int hammingDistance = CalculateHammingDistance(hash1, hash2);

        return hammingDistance <= acceptedHammingDistance;
    }

    private int CalculateHammingDistance(ulong hash1, ulong hash2)
    {
        ulong res = hash1 ^ hash2;
        return Convert.ToString((long)res, 2).Count(c => c == '1');
    }

    private ulong GetImageHash(string? imgBase64)
    {
        if (string.IsNullOrEmpty(imgBase64))
        {
            return 0;
        }

        byte[] imageBytes = Convert.FromBase64String(imgBase64);
        using Image<Rgba32> image = Image.Load<Rgba32>(imageBytes);

        byte[] grayscaledImage = GrayscaleImage(image);
        int averageValue = (int)grayscaledImage.Average(x => x);

        return GenerateHash(grayscaledImage, averageValue);
    }

    private byte[] GrayscaleImage(Image<Rgba32> imagePixels, int size = 8)
    {
        byte[] result = new byte[size * size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int step = size * i;
                result[step + j] = (byte)((imagePixels[j, i].R + imagePixels[j, i].G + imagePixels[j, i].B) / 3);
            }
        }

        return result;
    }

    private ulong GenerateHash(byte[] pixels, int averageGrayscaleValue)
    {
        ulong hash = 0;

        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i] > averageGrayscaleValue)
            {
                hash |= 1ul << i;
            }
        }

        return hash;
    }
}