using Newtonsoft.Json;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.XIntegrationTest.BlobServiceTests.Utils;
using Xunit;

namespace Streetcode.XIntegrationTest.BlobServiceTests
{
    public class FindFileInStorageAsBase64Tests : IClassFixture<BlobStorageFixture>, IDisposable
    {
        private readonly BlobStorageFixture _fixture;
        const string seededFileName = "find-as-base64-test";
        private string? _filePath;

        public FindFileInStorageAsBase64Tests()
        {
            _fixture = new BlobStorageFixture();
            _fixture.Seeding(seededFileName);
        }

        public void Dispose()
        {
            _filePath = Path.Combine(_fixture.blobPath, $"{seededFileName}.png");

            if (!string.IsNullOrEmpty(_filePath) && File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }

        [Theory]
        [InlineData($"{seededFileName}.png", "../../../BlobServiceTests/Utils/testData.json")]
        public void ShouldReturnValidBase64_FileExists(string validFileName, string testDataFilePath)
        {
            // Arrange
            string jsonContents = File.ReadAllText(testDataFilePath);
            Image jsonData = JsonConvert.DeserializeObject<Image>(jsonContents);
            string expactedBase64 = jsonData.Base64;

            // Act
            string result = _fixture.blobService.FindFileInStorageAsBase64(validFileName);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(result);
                Assert.Equal(expactedBase64, result);
            });
        }

        [Theory]
        [InlineData("name.gif")]
        public void ShouldReturnError_NoSuchFile(string nonExistingFileName)
        {
            // Arrange
            string result = "";

            // Act
            void action() => result = _fixture.blobService.FindFileInStorageAsBase64(nonExistingFileName);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.Throws<FileNotFoundException>(action);
                Assert.Empty(result);
            });
        }

        [Theory]
        [InlineData("not-valid-file-name")]
        public void ShouldThrowException_WhenInvalidFileName(string notValidFileName)
        {
            // Arrange
            string result = "";

            // Act
            void action() => result = _fixture.blobService.FindFileInStorageAsBase64(notValidFileName);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.Throws<IndexOutOfRangeException>(action);
                Assert.Empty(result);
            });
        }
    }
}
