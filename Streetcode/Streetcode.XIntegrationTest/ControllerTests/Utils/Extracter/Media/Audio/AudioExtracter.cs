using Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.Utils;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Media.Audio
{
    public class AudioExtracter
    {
        public static DAL.Entities.Media.Audio Extract(int audioId)
        {
            var blobFixture = new BlobStorageFixture();
            DAL.Entities.Media.Audio testAudio = blobFixture.SeedAudio(Guid.NewGuid().ToString());
            testAudio.Id = audioId;
            testAudio.BlobName += $".{testAudio.MimeType?.Split('/')[1]}";

            return BaseExtracter.Extract<DAL.Entities.Media.Audio>(testAudio, audio => audio.Id == audioId);
        }

        public static void Remove(DAL.Entities.Media.Audio entity)
        {
            var blobFixture = new BlobStorageFixture();
            blobFixture.blobService.DeleteFileInStorage(entity.BlobName);
            BaseExtracter.RemoveByPredicate<DAL.Entities.Media.Audio>(audio => audio.Id == entity.Id);
        }
    }
}
