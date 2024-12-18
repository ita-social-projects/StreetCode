using Newtonsoft.Json;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.Utils;
using Xunit;

namespace Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.FindFileTests
{
    public class FindFileInStorageAsBase64Tests : BlobServiceTestBase
    {
        public FindFileInStorageAsBase64Tests()
            : base(new BlobStorageFixture(), "find-as-base64-test")
        {
            this.Fixture.SeedImage(this.SeededFileName);
        }

        [Theory]
        [InlineData("png", "../../../ServiceTests/BlobServiceTests/Utils/testData.json")]
        public void ShouldReturnValidBase64_FileExists(string extension, string testDataFilePath)
        {
            // Arrange
            string validFileName = $"{this.SeededFileName}.{extension}";
            string jsonContents = File.ReadAllText(testDataFilePath);
            Image jsonData = JsonConvert.DeserializeObject<Image>(jsonContents) !;
            string expectedBase64 = jsonData.Base64!;

            // Act
            string result = this.Fixture.BlobService.FindFileInStorageAsBase64(validFileName);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(result);
                Assert.Equal(expectedBase64, result);
            });
        }

        [Theory]
        [InlineData("name.gif")]
        public void ShouldReturnError_NoSuchFile(string nonExistingFileName)
        {
            // Arrange
            string result = string.Empty;

            // Act
            void Action() => result = this.Fixture.BlobService.FindFileInStorageAsBase64(nonExistingFileName);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.Throws<FileNotFoundException>(Action);
                Assert.Empty(result);
            });
        }

        [Theory]
        [InlineData("not-valid-file-name")]
        public void ShouldThrowException_WhenInvalidFileName(string notValidFileName)
        {
            // Arrange
            string result = string.Empty;

            // Act
            void Action() => result = this.Fixture.BlobService.FindFileInStorageAsBase64(notValidFileName);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.Throws<IndexOutOfRangeException>(Action);
                Assert.Empty(result);
            });
        }
    }
}
