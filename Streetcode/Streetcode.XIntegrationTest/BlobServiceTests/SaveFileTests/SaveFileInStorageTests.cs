using Newtonsoft.Json;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.XIntegrationTest.BlobServiceTests.Utils;
using Xunit;

namespace Streetcode.XIntegrationTest.BlobServiceTests
{
    public class SaveFileInStorageTests : IClassFixture<BlobStorageFixture>, IDisposable
    {

        private readonly BlobStorageFixture _fixture;
        private string? _filePath;

        public SaveFileInStorageTests()
        {
            _fixture = new BlobStorageFixture();
        }

        public void Dispose()
        {
            if (!string.IsNullOrEmpty(_filePath) && File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }

        [Theory]
        [InlineData("../../../BlobServiceTests/Utils/testData.json")]
        public void ShouldSaveFileToBlobStorage_AllValid(string testDataFilePath)
        {
            // Arrange
            string jsonContents = File.ReadAllText(testDataFilePath);
            Image jsonData = JsonConvert.DeserializeObject<Image>(jsonContents);

            string fileName = jsonData.BlobName;
            string extension = jsonData.MimeType.Split('/')[1];
            string base64 = jsonData.Base64;

            // Act
            string hashBlobStorageName = _fixture.blobService.SaveFileInStorage(base64, fileName, extension);

            // Assert
            _filePath = Path.Combine(_fixture.blobPath, $"{hashBlobStorageName}.{extension}");
            Assert.Multiple(() =>
            {
                Assert.True(File.Exists(_filePath));
                Assert.False(string.IsNullOrEmpty(hashBlobStorageName));
                Assert.Equal($"{hashBlobStorageName}.{extension}", Path.GetFileName(_filePath));
            });
        }

        [Theory]
        [InlineData("NotvalidBase64", "test-file", "png")]
        public void ShouldThrowException_NotValidBase64(string base64, string fileName, string extension)
        {
            // Act
            void action() => _fixture.blobService.SaveFileInStorage(base64, fileName, extension);

            // Assert
            Assert.Throws<FormatException>(action);
        }
    }
}
