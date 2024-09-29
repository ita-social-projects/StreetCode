using Newtonsoft.Json;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.Utils;
using Xunit;

namespace Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.UpdateFileTest
{
    public class UpdateFileInStorageTests : BlobServiceTestBase
    {
        public UpdateFileInStorageTests()
            : base(new BlobStorageFixture(), "update-test")
        {
            this.Fixture.SeedImage(this.SeededFileName);
        }

        [Theory]
        [InlineData("updated-file", "../../../ServiceTests/BlobServiceTests/Utils/testData.json")]
        public void ShouldUpdateFileInStorage(string newBlobName, string testDataFilePath)
        {
            // Arrange
            string jsonContents = File.ReadAllText(testDataFilePath);
            Image jsonData = JsonConvert.DeserializeObject<Image>(jsonContents) !;

            string extension = jsonData.MimeType!.Split('/')[1];
            string base64 = jsonData.Base64!;
            string previousFullName = $"{this.SeededFileName}.{extension}";

            // Act
            string hashedResult = this.Fixture.BlobService.UpdateFileInStorage(previousFullName, base64, newBlobName, extension);
            string hashedFullName = $"{hashedResult}.{extension}";

            this.FilePath = Path.Combine(this.Fixture.BlobPath, $"{hashedFullName}");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.False(File.Exists($"{this.Fixture.BlobPath}{previousFullName}"));
                Assert.True(File.Exists($"{this.Fixture.BlobPath}{hashedFullName}"));
            });
        }
    }
}
