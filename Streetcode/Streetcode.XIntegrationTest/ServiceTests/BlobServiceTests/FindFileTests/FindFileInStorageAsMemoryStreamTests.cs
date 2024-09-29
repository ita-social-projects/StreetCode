using Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.Utils;
using Xunit;

namespace Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.FindFileTests
{
    public class FindFileInStorageAsMemoryStreamTests : BlobServiceTestBase
    {
        public FindFileInStorageAsMemoryStreamTests()
            : base(new BlobStorageFixture(), "find-as-memory-stream-test")
        {
            this.Fixture.SeedImage(this.SeededFileName);
        }

        [Theory]
        [InlineData("png")]
        public void ShouldReturnValidMemoryStream_FileExists(string extension)
        {
            // Act
            string validFileName = $"{this.SeededFileName}.{extension}";
            MemoryStream memoryStream = this.Fixture.BlobService.FindFileInStorageAsMemoryStream(validFileName);

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
            MemoryStream? memoryStream = null;

            // Act
            void Action() => memoryStream = this.Fixture.BlobService.FindFileInStorageAsMemoryStream(nonExistingFileName);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.Throws<FileNotFoundException>(Action);
                Assert.Null(memoryStream);
            });
        }

        [Theory]
        [InlineData("not-valid-file-name")]
        public void ShouldThrowException_WhenInvalidFileName(string notValidFileName)
        {
            // Arrange
            MemoryStream? memoryStream = null;

            // Act
            void Action() => memoryStream = this.Fixture.BlobService.FindFileInStorageAsMemoryStream(notValidFileName);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.Throws<IndexOutOfRangeException>(Action);
                Assert.Null(memoryStream);
            });
        }
    }
}