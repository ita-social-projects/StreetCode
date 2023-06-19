using Microsoft.Extensions.Options;
using Streetcode.XIntegrationTest.BlobServiceTests.Utils;
using Xunit;
using System.IO;

namespace Streetcode.XIntegrationTest.Tests
{
    public class BlobStorageFixtureTests
    {
        [Fact]
        public void Seed_ShouldCreateTestFilesInBlobStorage()
        {
            // Arrange
            var blobPath = "../../../BlobServiceTests/Utils/BlobStorageTest/";
            var fixture = new BlobStorageFixture(blobPath);

            // Act
            fixture.Seed();
            
            // Assert
            Assert.True(Directory.EnumerateFiles(blobPath).Any());
        }
    }
}
