using Newtonsoft.Json;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.Utils;
using Xunit;

namespace Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.SaveFileTests
{
    public class SaveFileInStorageBase64Tests : BlobServiceTestBase
    {
        public SaveFileInStorageBase64Tests()
            : base(new BlobStorageFixture(), "saved-image-base64")
        {
        }

        [Theory]
        [InlineData("../../../ServiceTests/BlobServiceTests/Utils/testData.json")]
        public void ShouldSaveFileToBlobStorageBase64_AllValid(string testDataFilePath)
        {
            // Arrange
            string jsonContents = File.ReadAllText(testDataFilePath);
            Image jsonData = JsonConvert.DeserializeObject<Image>(jsonContents) !;

            string fileName = this.SeededFileName;
            string extension = jsonData.MimeType!.Split('/')[1];
            string base64 = jsonData.Base64!;

            // Act
            this.Fixture.BlobService.SaveFileInStorageBase64(base64, fileName, extension);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.True(File.Exists(this.FilePath));
                Assert.Equal($"{fileName}.{extension}", Path.GetFileName(this.FilePath));
            });
        }

        [Theory]
        [InlineData("NotvalidBase64", "test-file", "png")]
        public void ShouldThrowException_NotValidBase64(string base64, string fileName, string extension)
        {
            // Act
            void Action() => this.Fixture.BlobService.SaveFileInStorageBase64(base64, fileName, extension);

            // Assert
            Assert.Throws<FormatException>(Action);
        }
    }
}