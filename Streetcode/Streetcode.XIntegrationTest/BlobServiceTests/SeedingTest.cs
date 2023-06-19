using Streetcode.XIntegrationTest.BlobServiceTests.Utils;
using Xunit;

namespace Streetcode.XIntegrationTest.Tests
{
    public class BlobStorageFixtureTests
    {
        [Fact]
        public void Seed_ShouldCreateTestFilesInBlobStorage()
        {
            // Arrange
            var blobPath = "../../BlobServiceTests/";
            var fixture = new BlobStorageFixture();

            // Act
            fixture.Seed();
            
            // Assert
            Assert.True(Directory.EnumerateFiles(blobPath).Any());
        }
    }
}
