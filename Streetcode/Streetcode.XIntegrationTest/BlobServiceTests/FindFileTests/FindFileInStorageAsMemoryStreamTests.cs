using Streetcode.XIntegrationTest.BlobServiceTests.Utils;
using Xunit;

namespace Streetcode.XIntegrationTest.BlobServiceTests
{
    public class FindFileInStorageAsMemoryStreamTests : BlobServiceTestBase
    {
        public FindFileInStorageAsMemoryStreamTests() : base(new BlobStorageFixture(), "find-as-memory-stream-test")
        {
            _fixture.Seeding(_seededFileName);
        }

        [Theory]
        [InlineData("png")]
        public void ShouldReturnValidMemoryStream_FileExists(string extension)
        {
            // Act
            string validFileName = $"{_seededFileName}.{extension}";
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