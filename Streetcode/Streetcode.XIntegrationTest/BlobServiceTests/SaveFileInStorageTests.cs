using Microsoft.Extensions.Options;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Services.BlobStorageService;
using Xunit;

namespace Streetcode.XIntegrationTest.BlobServiceTests
{
    public class SaveFileInStorageTests
    {
        private readonly IBlobService blobService;
        private readonly IOptions<BlobEnvironmentVariables> environmentVariables;
        private readonly string blobPath = "../../BlobStorageTest/";
        private readonly string blobKey = "somethingForTest";

        public SaveFileInStorageTests()
        {
            environmentVariables = Options.Create(new BlobEnvironmentVariables());
            environmentVariables.Value.BlobStorePath = blobPath;
            environmentVariables.Value.BlobStoreKey = blobKey;

            blobService = new BlobService(environmentVariables);
        }

        [Fact]
        public void SaveFileInStorage_ReturnsSuccess()
        {
            
        }

        [Fact]
        public void SaveFileInStorage_ReturnsError()
        {

        }

    }
}
