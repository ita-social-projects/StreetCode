using Xunit;

namespace Streetcode.XIntegrationTest.BlobServiceTests.Utils
{
    public class BlobServiceTestBase : IClassFixture<BlobStorageFixture>, IDisposable
    {
        protected readonly BlobStorageFixture _fixture;
        protected string _seededFileName;
        protected string _filePath;

        public BlobServiceTestBase(BlobStorageFixture fixture, string seededFileName="")
        {
            _fixture = fixture;
            _seededFileName = seededFileName;
            _filePath = Path.Combine(_fixture.blobPath, $"{_seededFileName}.png");
        }

        public void Dispose()
        {
            if (!string.IsNullOrEmpty(_filePath) && File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }
    }
}
