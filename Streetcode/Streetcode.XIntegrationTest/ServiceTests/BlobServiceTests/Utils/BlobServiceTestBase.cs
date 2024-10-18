using Xunit;

namespace Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.Utils
{
    public class BlobServiceTestBase : IClassFixture<BlobStorageFixture>, IDisposable
    {
        private bool disposed;

        public BlobServiceTestBase(BlobStorageFixture fixture, string seededFileName = "")
        {
            this.Fixture = fixture;
            this.SeededFileName = seededFileName;
            this.FilePath = Path.Combine(this.Fixture.BlobPath, $"{this.SeededFileName}.png");
        }

        protected string FilePath { get; set; }

        protected string SeededFileName { get; set; }

        protected BlobStorageFixture Fixture { get; set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                if (!string.IsNullOrEmpty(this.FilePath) && File.Exists(this.FilePath))
                {
                    File.Delete(this.FilePath);
                }

                this.disposed = true;
            }
        }
    }
}
