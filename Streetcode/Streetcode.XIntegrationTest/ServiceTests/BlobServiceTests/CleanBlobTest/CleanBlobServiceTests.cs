//using Streetcode.DAL.Entities.Media.Images;
//using Streetcode.XIntegrationTest.Base;
//using Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.Utils;
//using Xunit;

//namespace Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.CleanBlobTest
//{
//    public class CleanBlobServiceTests : BlobServiceTestBase, IClassFixture<TestDBFixture>
//    {
//        private TestDBFixture DbFixture { get; }

//        public CleanBlobServiceTests(TestDBFixture fixture) : base(new BlobStorageFixture())
//        {
//            DbFixture = fixture;
//        }

//        [Theory]
//        [InlineData("ED_kNMjZkMDz6_syM5klb8HGDyfU72Q6Sdz_Y4DmCJ8=.png")]
//        public async Task ShouldRemoveUnusedFilesFromBlobStorage(string blobName)
//        {
//            // Arrange
//            await _fixture.DbAndStorageSeeding();
//            Image imgToRemove = _fixture.TestDbContext.Images.FirstOrDefault(x => x.BlobName == blobName);
//            _fixture.TestDbContext.Images.Remove(imgToRemove);
//            await _fixture.TestDbContext.SaveChangesAsync();

//            // Act
//            await _fixture.blobService.CleanBlobStorage();

//            // Assert
//            Assert.False(File.Exists(_fixture.blobPath + blobName));
//        }

//        [Theory]
//        [InlineData("ED_kNMjZkMDz6_syM5klb8HGDyfU72Q6Sdz_Y4DmCJ8=.png")]
//        public async Task NothingToRemoveFromBlobStorage(string blobName)
//        {
//            // Arrange
//            await _fixture.DbAndStorageSeeding();
//            Image imgToRemove = _fixture.TestDbContext.Images.FirstOrDefault(x => x.BlobName == blobName);

//            // Act
//            await _fixture.blobService.CleanBlobStorage();

//            // Assert
//            Assert.True(File.Exists(_fixture.blobPath + blobName));
//        }
//    }
//}
