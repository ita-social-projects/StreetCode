using Microsoft.Extensions.Options;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Services.BlobStorageService;

namespace Streetcode.XIntegrationTest.BlobServiceTests.Utils
{
    public class BlobStorageFixture
    {
        private readonly IBlobService blobService;
        private readonly IOptions<BlobEnvironmentVariables> environmentVariables;
        private bool isInitialized;

        public BlobStorageFixture(string blobPath = "../../BlobStorageTest/", string blobKey = "somethingForTest")
        {
            environmentVariables = Options.Create(new BlobEnvironmentVariables());
            environmentVariables.Value.BlobStorePath = blobPath;
            environmentVariables.Value.BlobStoreKey = blobKey;
            isInitialized = false;

            blobService = new BlobService(environmentVariables);
        }

        public void Seed()
        {
            if (!isInitialized)
            { 
                
            }
        }
    }
}
