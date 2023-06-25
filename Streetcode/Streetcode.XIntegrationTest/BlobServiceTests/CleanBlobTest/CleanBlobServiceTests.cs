using Streetcode.DAL.Entities.Media.Images;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.BlobServiceTests.Utils;
using Xunit;

namespace Streetcode.XIntegrationTest.BlobServiceTests
{
    public class CleanBlobServiceTests : BlobServiceTestBase, IClassFixture<TestDBFixture>
    {
        private TestDBFixture DbFixture { get; }

        public CleanBlobServiceTests(TestDBFixture fixture) : base(new BlobStorageFixture())
        {
            DbFixture = fixture;
        }

        [Theory]
        [InlineData("Parimatch")]
        public async Task ShouldRemoveUnusedFilesFromBlobStorage(string title)
        {
            // Arrange
            await _fixture.DbAndStorageSeeding();
            Image imgToRemove = _fixture.TestDbContext.Images.FirstOrDefault(x => x.Title == title);
            string blobName = imgToRemove.BlobName;
            _fixture.TestDbContext.Images.Remove(imgToRemove);
            await _fixture.TestDbContext.SaveChangesAsync();

            // Act
            await _fixture.blobService.CleanBlobStorage();

            // Assert
            Assert.False(File.Exists(_fixture.blobPath + blobName));
        }

        [Theory]
        [InlineData("Parimatch")]
        public async Task NothingToRemoveFromBlobStorage(string title)
        {
            // Arrange
            await _fixture.DbAndStorageSeeding();
            Image imgToRemove = _fixture.TestDbContext.Images.FirstOrDefault(x => x.Title == title);
            string blobName = imgToRemove.BlobName;

            // Act
            await _fixture.blobService.CleanBlobStorage();

            // Assert
            Assert.True(File.Exists(_fixture.blobPath + blobName));
        }
    }
}
