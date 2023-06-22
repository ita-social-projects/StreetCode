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

        // rewrite
        [Theory]
        [InlineData("delete-test", "png")]
        public void ShouldDeleteFileFromStorage_ExistingFile(string fileName, string extension)
        {
            // Arrange
            string filePath = Path.Combine(_fixture.blobPath, fileName);

            // Act
            _fixture.blobService.DeleteFileInStorage($"{fileName}.{extension}");

            // Assert
            Assert.False(File.Exists(filePath));
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
