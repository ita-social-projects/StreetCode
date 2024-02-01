using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.Utils;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.Image
{
    public class ImageExtracter
    {
        public static DAL.Entities.Media.Images.Image Extract(int imageId)
        {
            StreetcodeContent testStreetcode = StreetcodeContentExtracter
                .Extract(imageId, imageId, Guid.NewGuid().ToString());
            var blobFixture = new BlobStorageFixture();
            DAL.Entities.Media.Images.Image testImage = blobFixture.SeedImage(Guid.NewGuid().ToString());
            testImage.Id = imageId;
            testImage.BlobName += $".{testImage.MimeType?.Split('/')[1]}";
            testImage.Streetcodes = new List<StreetcodeContent>() { testStreetcode };

            return BaseExtracter.Extract<DAL.Entities.Media.Images.Image>(testImage, image => image.Id == imageId);
        }

        public static void Remove(DAL.Entities.Media.Images.Image entity)
        {
            var blobFixture = new BlobStorageFixture();
            blobFixture.blobService.DeleteFileInStorage(entity.BlobName);
            BaseExtracter.RemoveByPredicate<DAL.Entities.Media.Images.Image>(image => image.Id == entity.Id);
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
