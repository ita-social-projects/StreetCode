namespace Streetcode.BLL.Interfaces.ImageComparator;

public interface IImageComparatorService
{
    ulong SetImageHash(string? imgBase64);
}