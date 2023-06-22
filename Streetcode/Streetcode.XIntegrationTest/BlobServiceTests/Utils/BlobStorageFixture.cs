using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Streetcode.BLL.Services.BlobStorageService;
using Streetcode.DAL.Entities.Media.Images;
using System.Text;

namespace Streetcode.XIntegrationTest.BlobServiceTests.Utils
{
    public class BlobStorageFixture : IntegrationTestBase
    {
        public readonly BlobService blobService;
        private readonly IOptions<BlobEnvironmentVariables> environmentVariables;
        public readonly string blobPath;
        private readonly string blobKey;

        public BlobStorageFixture()
        {
            environmentVariables = Options.Create(new BlobEnvironmentVariables 
            { 
                BlobStoreKey = Configuration.GetValue<string>("Blob:BlobStoreKey"),
                BlobStorePath = Configuration.GetValue<string>("Blob:BlobStorePath")
            });
            blobPath = environmentVariables.Value.BlobStorePath;
            blobKey = environmentVariables.Value.BlobStoreKey;
            blobService = new BlobService(environmentVariables);
            Directory.CreateDirectory(blobPath);
        }

        public void Seeding(string givenBlobName)
        {
            string initialDataImagePath = "../../../BlobServiceTests/Utils/testData.json";
            string imageJson = File.ReadAllText(initialDataImagePath, Encoding.UTF8);
            Image imgfromJson = JsonConvert.DeserializeObject<Image>(imageJson);
            imgfromJson.BlobName = givenBlobName;
            string filePath = Path.Combine(blobPath, imgfromJson.BlobName);
            if (!File.Exists(filePath))
            {
                blobService.SaveFileInStorageBase64(imgfromJson.Base64, imgfromJson.BlobName, imgfromJson.MimeType.Split('/')[1]);
            }
        }
    }
}
