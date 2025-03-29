using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Streetcode.BLL.Interfaces.ImageComparator;

namespace Streetcode.BLL.Services.ImageComparator;

public class ImageHashGeneratorService : IImageHashGeneratorService
{
    private const int FixedWidth = 9;
    private const int FixedHeight = 8;

    public ulong SetImageHash(string? imgBase64)
    {
        if (string.IsNullOrEmpty(imgBase64))
        {
            return 0;
        }

        byte[] imageBytes = Convert.FromBase64String(imgBase64);
        using Image<Rgba32> image = Image.Load<Rgba32>(imageBytes);

        image.Mutate(x => x.Resize(FixedWidth, FixedHeight));
        byte[] grayscaledImage = GrayscaleImage(image);

        return GenerateHash(grayscaledImage);
    }

    private byte[] GrayscaleImage(Image<Rgba32> imagePixels)
    {
        byte[] result = new byte[FixedWidth * FixedHeight];
        for (int y = 0; y < FixedHeight; y++)
        {
            for (int x = 0; x < FixedWidth; x++)
            {
                int step = 9 * y;
                result[step + x] = (byte)((imagePixels[x, y].R + imagePixels[x, y].G + imagePixels[x, y].B) / 3);
            }
        }

        return result;
    }

    private ulong GenerateHash(byte[] pixels)
    {
        ulong hash = 0;

        for (int y = 0; y < FixedHeight; y++)
        {
            for (int x = 0; x < FixedWidth - 1; x++)
            {
                int step = 9 * y;

                if (pixels[x + step] > pixels[x + step + 1])
                {
                    hash |= 1ul << (x + (8 * y));
                }
            }
        }

        return hash;
    }
}