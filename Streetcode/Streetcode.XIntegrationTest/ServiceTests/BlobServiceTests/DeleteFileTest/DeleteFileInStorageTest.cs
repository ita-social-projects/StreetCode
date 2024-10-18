﻿using Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.Utils;
using Xunit;

namespace Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.DeleteFileTest
{
    public class DeleteFileInStorageTest : BlobServiceTestBase
    {
        public DeleteFileInStorageTest()
            : base(new BlobStorageFixture(), "delete-test")
        {
            this.Fixture.SeedImage(this.SeededFileName);
        }

        [Theory]
        [InlineData("png")]
        public void ShouldDeleteFileFromStorage_ExistingFile(string extension)
        {
            // Arrange
            string fileName = $"{this.SeededFileName}.{extension}";

            // Act
            this.Fixture.BlobService.DeleteFileInStorage(fileName);

            // Assert
            Assert.False(File.Exists(this.FilePath));
        }

        // [Theory]
        // [InlineData("invalid|file")]
        // public void ShouldThrowException_EmptyFileName(string fileName)
        // {
        //    // Act
        //    void action() => _fixture.blobService.DeleteFileInStorage(fileName);

        // // Assert
        //    Assert.Throws<IOException>(action);
        // }
    }
}
