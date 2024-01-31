using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Partners;
using Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.Utils;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.ImageExtracter
{
    public class ImageExtracter
    {
        public static Image Extract(int imageId)
        {
            var blobFixture = new BlobStorageFixture();
            Image testImage = blobFixture.SeedImage(Guid.NewGuid().ToString());
            testImage.Id = imageId;
            testImage.BlobName += $".{testImage.MimeType?.Split('/')[1]}";

            return BaseExtracter.Extract<Image>(testImage, image => image.Id == imageId);
        }

        public static void Remove(Image entity)
        {
            BaseExtracter.RemoveByPredicate<Image>(image => image.Id == entity.Id);
        }
    }
}
