using Streetcode.XIntegrationTest.BlobServiceTests.Utils;
using Xunit;

namespace Streetcode.XIntegrationTest.BlobServiceTests
{
    public class DeleteFileInStorageTest : BlobServiceTestBase
    {
        public DeleteFileInStorageTest() : base(new BlobStorageFixture(), "delete-test")
        {
            _fixture.Seeding(_seededFileName);
        }

        [Theory]
        [InlineData("png")]
        public void ShouldDeleteFileFromStorage_ExistingFile(string extension)
        {
            // Arrange
            string fileName = $"{_seededFileName}.{extension}";

            // Act
            _fixture.blobService.DeleteFileInStorage(fileName);

            // Assert
            Assert.False(File.Exists(_filePath));
        }

        [Theory]
        [InlineData("invalid|file")]
        public void ShouldThrowException_EmptyFileName(string fileName)
        {
            // Act
            void action() => _fixture.blobService.DeleteFileInStorage(fileName);

            // Assert
            Assert.Throws<IOException>(action);
        }
    }
}
