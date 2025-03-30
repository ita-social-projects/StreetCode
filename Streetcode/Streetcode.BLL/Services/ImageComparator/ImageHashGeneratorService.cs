using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Streetcode.BLL.Interfaces.ImageComparator;

namespace Streetcode.BLL.Services.ImageComparator;

public class ImageHashGeneratorService : IImageHashGeneratorService
{
    private const int FixedWidth = 9;
    private const int FixedHeight = 8;

    public ulong GenerateImageHash(string? imgBase64)
    {
        if (string.IsNullOrEmpty(imgBase64))
        {
            return 0;
        }

        byte[] imageBytes = Convert.FromBase64String(imgBase64);
        using Image<Rgba32> image = Image.Load<Rgba32>(imageBytes);

        image.Mutate(x => x.Resize(FixedWidth, FixedHeight));

        return GenerateHash(image);
    }

    private ulong GenerateHash(Image<Rgba32> image)
    {
        ulong rHash = 0;
        ulong gHash = 0;
        ulong bHash = 0;

        for (int y = 0; y < FixedHeight; y++)
        {
            for (int x = 0; x < FixedWidth - 1; x++)
            {
                if (image[x, y].R > image[x + 1, y].R)
                {
                    rHash |= 1ul << (x + (8 * y));
                }

                if (image[x, y].G > image[x + 1, y].G)
                {
                    gHash |= 1ul << (x + (8 * y));
                }

                if (image[x, y].B > image[x + 1, y].B)
                {
                    bHash |= 1ul << (x + (8 * y));
                }
            }
        }

        ulong resultHash = rHash ^ RotateHashLeft(gHash, 21) ^ RotateHashLeft(bHash, 42);

        return resultHash;
    }

    private ulong RotateHashLeft(ulong hashToRotate, int shift)
    {
        return (hashToRotate << shift) | (hashToRotate >> (64 - shift));
    }
}