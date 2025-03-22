namespace Streetcode.BLL.Interfaces.ImageComparator;

public interface IImageComparatorService
{
    bool CompareImages(string? img1Base64, string? img2Base64, int acceptedHammingDistance);
}