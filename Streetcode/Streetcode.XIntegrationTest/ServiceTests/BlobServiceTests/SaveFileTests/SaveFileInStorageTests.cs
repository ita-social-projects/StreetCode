using Newtonsoft.Json;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.Utils;
using Xunit;

namespace Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.SaveFileTests
{
    public class SaveFileInStorageTests : BlobServiceTestBase
    {
        public SaveFileInStorageTests()
            : base(new BlobStorageFixture(), "saved-image")
        {
        }

        [Theory]
        [InlineData("../../../ServiceTests/BlobServiceTests/Utils/testData.json")]
        public void ShouldSaveFileToBlobStorage_AllValid(string testDataFilePath)
        {
            // Arrange
            string jsonContents = File.ReadAllText(testDataFilePath);
            Image jsonData = JsonConvert.DeserializeObject<Image>(jsonContents) !;

            string fileName = this.SeededFileName;
            string extension = jsonData.MimeType!.Split('/')[1];
            string base64 = jsonData.Base64!;

            // Act
            string hashBlobStorageName = this.Fixture.BlobService.SaveFileInStorage(base64, fileName, extension);

            // Assert
            this.FilePath = Path.Combine(this.Fixture.BlobPath, $"{hashBlobStorageName}.{extension}");
            Assert.Multiple(() =>
            {
                Assert.True(File.Exists(this.FilePath));
                Assert.False(string.IsNullOrEmpty(hashBlobStorageName));
                Assert.Equal($"{hashBlobStorageName}.{extension}", Path.GetFileName(this.FilePath));
            });
        }

        [Theory]
        [InlineData("NotvalidBase64", "test-file", "png")]
        public void ShouldThrowException_NotValidBase64(string base64, string fileName, string extension)
        {
            // Act
            void Action() => this.Fixture.BlobService.SaveFileInStorage(base64, fileName, extension);

            // Assert
            Assert.Throws<FormatException>(Action);
        }
    }
}
