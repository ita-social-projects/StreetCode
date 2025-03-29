namespace Streetcode.BLL.Interfaces.ImageComparator;

public interface IImageHashGeneratorService
{
    ulong SetImageHash(string? imgBase64);
}