using Streetcode.XIntegrationTest.BlobServiceTests.Utils;
using Xunit;

namespace Streetcode.XIntegrationTest.BlobServiceTests
{
    public class FindFileInStorageAsMemoryStreamTests : IClassFixture<BlobStorageFixture>, IDisposable
    {
        private readonly BlobStorageFixture _fixture;
        const string seededFileName = "find-as-memory-stream-test";
        private string? _filePath;

        public FindFileInStorageAsMemoryStreamTests()
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
        [InlineData($"{seededFileName}.png")]
        public void ShouldReturnValidMemoryStream_FileExists(string validFileName)
        {
            // Act
            MemoryStream memoryStream = _fixture.blobService.FindFileInStorageAsMemoryStream(validFileName);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(memoryStream);
                Assert.True(memoryStream.Length > 0);
            });
        }

        [Theory]
        [InlineData("ba97sDiJCukzoLUoYZhE4fy=.gif")]
        public void ShouldReturnError_NoSuchFile(string nonExistingFileName)
        {
            // Arrange
            MemoryStream memoryStream = null;

            // Act
            void action() => memoryStream = _fixture.blobService.FindFileInStorageAsMemoryStream(nonExistingFileName);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.Throws<FileNotFoundException>(action);
                Assert.Null(memoryStream);
            });
        }

        [Theory]
        [InlineData("not-valid-file-name")]
        public void ShouldThrowException_WhenInvalidFileName(string notValidFileName)
        {
            // Arrange
            MemoryStream memoryStream = null;

            // Act
            void action() => memoryStream = _fixture.blobService.FindFileInStorageAsMemoryStream(notValidFileName);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.Throws<IndexOutOfRangeException>(action);
                Assert.Null(memoryStream);
            });
        }
    }
}