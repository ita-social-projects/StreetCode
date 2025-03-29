namespace Streetcode.BLL.Interfaces.ImageComparator;

public interface IImageHashGeneratorService
{
    ulong GenerateImageHash(string? imgBase64);
}