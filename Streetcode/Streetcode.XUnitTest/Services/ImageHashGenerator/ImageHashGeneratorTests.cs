using Streetcode.BLL.Services.ImageComparator;
using Xunit;

namespace Streetcode.XUnitTest.Services.ImageHashGenerator;

public class ImageHashGeneratorTests
{
    [Fact]
    private void GenerateImageHash_ValidBase64()
    {
        string base64 = "iVBORw0KGgoAAAANSUhEUgAAAAoAAAAKCAYAAACNMs+9AAAADklEQVR4nGNgGAWDEwAAAZoAAR2CVqgAAAAASUVORK5CYII=";

        var hashGenerator = new ImageHashGeneratorService();
        ulong hash = hashGenerator.GenerateImageHash(base64);

        Assert.True(hash >= 0);
    }

    [Fact]
    private void GenerateImageHash_InvalidBase64()
    {
        string base64 = "invalid_base64";

        var hashGenerator = new ImageHashGeneratorService();
        ulong hash = hashGenerator.GenerateImageHash(base64);

        Assert.True(hash == 0);
    }

    [Fact]
    private void GenerateImageHash_EmptyBase64()
    {
        string base64 = string.Empty;

        var hashGenerator = new ImageHashGeneratorService();
        ulong hash = hashGenerator.GenerateImageHash(base64);

        Assert.True(hash == 0);
    }
}