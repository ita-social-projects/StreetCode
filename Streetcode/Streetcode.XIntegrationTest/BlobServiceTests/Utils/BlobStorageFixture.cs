using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Services.BlobStorageService;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Entities.Media.Images;
using System.Text;

namespace Streetcode.XIntegrationTest.BlobServiceTests.Utils
{
    public class BlobStorageFixture
    {
        private readonly BlobService blobService;
        private readonly IOptions<BlobEnvironmentVariables> environmentVariables;
        private readonly string blobPath;
        private readonly string blobKey;

        public BlobStorageFixture()
        {
            (string, string) blobInfo = Configuration();
            environmentVariables = Options.Create(new BlobEnvironmentVariables 
            { 
                BlobStoreKey = blobInfo.Item1, BlobStorePath = blobInfo.Item2
            });
            blobPath = environmentVariables.Value.BlobStorePath;
            blobKey = environmentVariables.Value.BlobStoreKey;
            blobService = new BlobService(environmentVariables);
            Directory.CreateDirectory(blobPath);
        }
        
        private static (string, string) Configuration()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var relativePath = "../../../../Streetcode.WebApi/appsettings.IntegrationTests.json";
            var configPath = Path.GetFullPath(Path.Combine(currentDirectory, relativePath));

            var config = new ConfigurationBuilder()
                .AddJsonFile(configPath)
                .Build();

            var blobSection = config.GetSection("Blob");
            string blobStorePath = blobSection["BlobStorePath"];
            string blobStoreKey = blobSection["BlobStoreKey"];
            return (blobStoreKey, blobStorePath);
        }

        public void Seed()
        {
            if(!Directory.EnumerateFiles(blobPath).Any())
            {
                string initialDataImagePath = "../../../../Streetcode.DAL/InitialData/images.json";
                string initialDataAudioPath = "../../../../Streetcode.DAL/InitialData/audios.json";

                string imageJson = File.ReadAllText(initialDataImagePath, Encoding.UTF8);
                string audiosJson = File.ReadAllText(initialDataAudioPath, Encoding.UTF8);

                var imgfromJson = JsonConvert.DeserializeObject<List<Image>>(imageJson);
                var audiosfromJson = JsonConvert.DeserializeObject<List<Audio>>(audiosJson);

                foreach (var img in imgfromJson)
                {
                    string filePath = Path.Combine(blobPath, img.BlobName);
                    if (!File.Exists(filePath))
                    {
                        blobService.SaveFileInStorageBase64(img.Base64, img.BlobName.Split('.')[0], img.BlobName.Split('.')[1]);
                    }
                }

                foreach (var audio in audiosfromJson)
                {
                    string filePath = Path.Combine(blobPath, audio.BlobName);
                    if (!File.Exists(filePath))
                    {
                        blobService.SaveFileInStorageBase64(audio.Base64, audio.BlobName.Split('.')[0], audio.BlobName.Split('.')[1]);
                    }
                }
            }
        }
    }
}
