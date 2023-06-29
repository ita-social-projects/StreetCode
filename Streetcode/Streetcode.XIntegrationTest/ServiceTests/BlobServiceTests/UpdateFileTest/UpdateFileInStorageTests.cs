using Newtonsoft.Json;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.Utils;
using Xunit;

namespace Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.UpdateFileTest
{
    public class UpdateFileInStorageTests : BlobServiceTestBase
    {
        public UpdateFileInStorageTests() : base(new BlobStorageFixture(), "update-test")
        {
            _fixture.Seeding(_seededFileName);
        }

        [Theory]
        [InlineData("updated-file", "../../../ServiceTests/BlobServiceTests/Utils/testData.json")]
        public void ShouldUpdateFileInStorage(string newBlobName, string testDataFilePath)
        {
            // Arrange
            string jsonContents = File.ReadAllText(testDataFilePath);
            Image jsonData = JsonConvert.DeserializeObject<Image>(jsonContents);

            string extension = jsonData.MimeType.Split('/')[1];
            string base64 = jsonData.Base64;
            string previousFullName = $"{_seededFileName}.{extension}";

            // Act
            string hashedResult = _fixture.blobService.UpdateFileInStorage(previousFullName, base64, newBlobName, extension);
            string hashedFullName = $"{hashedResult}.{extension}";

            _filePath = Path.Combine(_fixture.blobPath, $"{hashedFullName}");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.False(File.Exists($"{_fixture.blobPath}{previousFullName}"));
                Assert.True(File.Exists($"{_fixture.blobPath}{hashedFullName}"));
            });
        }

    }
}
