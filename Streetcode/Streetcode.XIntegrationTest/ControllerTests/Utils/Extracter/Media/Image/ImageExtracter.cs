using Streetcode.DAL.Entities.Media.Images;
using Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.Utils;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.Image
{
    public static class ImageExtracter
    {
        public static DAL.Entities.Media.Images.Image Extract(int imageId)
        {
            var blobFixture = new BlobStorageFixture();
            DAL.Entities.Media.Images.Image testImage = blobFixture.SeedImage(Guid.NewGuid().ToString());
            testImage.Id = imageId;
            testImage.BlobName += $".{testImage.MimeType?.Split('/')[1]}";

            return BaseExtracter.Extract(testImage, image => image.Id == imageId);
        }

        public static void Remove(DAL.Entities.Media.Images.Image entity)
        {
            var blobFixture = new BlobStorageFixture();
            BaseExtracter.RemoveByPredicate<DAL.Entities.Media.Images.Image>(image => image.Id == entity.Id);
            blobFixture.BlobService.DeleteFileInStorage(entity.BlobName!);
        }

        public static void AddStreetcodeImage(int streetcodeId, int imageId)
        {
            StreetcodeImage streetcodeImage = new StreetcodeImage()
            {
                StreetcodeId = streetcodeId,
                ImageId = imageId,
            };
            BaseExtracter.Extract<StreetcodeImage>(
                streetcodeImage,
                strImg => strImg.ImageId == imageId && strImg.StreetcodeId == streetcodeId,
                hasIdentity: false);
        }
    }
}
