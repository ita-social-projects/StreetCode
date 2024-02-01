using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.Utils;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.ImageExtracter
{
    public class ImageExtracter
    {
        public static Image Extract(int imageId)
        {
            StreetcodeContent testStreetcode = StreetcodeContentExtracter
                .Extract(imageId, imageId, Guid.NewGuid().ToString());
            var blobFixture = new BlobStorageFixture();
            Image testImage = blobFixture.SeedImage(Guid.NewGuid().ToString());
            testImage.Id = imageId;
            testImage.BlobName += $".{testImage.MimeType?.Split('/')[1]}";
            testImage.Streetcodes = new List<StreetcodeContent>() { testStreetcode };

            return BaseExtracter.Extract<Image>(testImage, image => image.Id == imageId);
        }

        public static void Remove(Image entity)
        {
            var blobFixture = new BlobStorageFixture();
            blobFixture.blobService.DeleteFileInStorage(entity.BlobName);
            BaseExtracter.RemoveByPredicate<Image>(image => image.Id == entity.Id);
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
