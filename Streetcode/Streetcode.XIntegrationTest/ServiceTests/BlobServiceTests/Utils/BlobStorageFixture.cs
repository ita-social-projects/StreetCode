using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Streetcode.BLL.Services.BlobStorageService;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Persistence;

namespace Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.Utils
{
    public class BlobStorageFixture : IntegrationTestBase
    {
        private readonly IOptions<BlobEnvironmentVariables> environmentVariables;

        public BlobStorageFixture()
        {
            this.environmentVariables = Options.Create(new BlobEnvironmentVariables
            {
                BlobStoreKey = this.Configuration.GetValue<string>("Blob:BlobStoreKey") !,
                BlobStorePath = this.Configuration.GetValue<string>("Blob:BlobStorePath") !,
            });
            this.BlobPath = this.environmentVariables.Value.BlobStorePath;

            this.BlobService = new BlobService(this.environmentVariables); // add repo
            Directory.CreateDirectory(this.BlobPath);
        }

        public BlobService BlobService { get; }

        public string BlobPath { get; private set; }

        public StreetcodeDbContext TestDbContext { get; private set; } = null!;

        public Image SeedImage(string givenBlobName)
        {
            string testDataImagePath = "../../../ServiceTests/BlobServiceTests/Utils/testData.json";
            string imageJson = File.ReadAllText(testDataImagePath, Encoding.UTF8);
            Image imgfromJson = JsonConvert.DeserializeObject<Image>(imageJson) !;
            imgfromJson.BlobName = givenBlobName;
            this.SaveFileIfNotExist(imgfromJson.Base64!, imgfromJson.BlobName, imgfromJson.MimeType!.Split('/')[1]);
            return imgfromJson;
        }

        public Audio? SeedAudio(string givenBlobName)
        {
            string initialDataAudioPath = "../../../../Streetcode.DAL/InitialData/audios.json";
            string audiosJson = File.ReadAllText(initialDataAudioPath, Encoding.UTF8);
            List<Audio> audiosfromJson = JsonConvert.DeserializeObject<List<Audio>>(audiosJson) !;
            if (audiosfromJson != null && audiosfromJson.Count > 1)
            {
                audiosfromJson[0].BlobName = givenBlobName;
                this.SaveFileIfNotExist(audiosfromJson[0].Base64!, audiosfromJson[0].BlobName!, audiosfromJson[0].MimeType!.Split('/')[1]);
                return audiosfromJson[0];
            }

            return null;
        }

        public async Task DbAndStorageSeeding()
        {
            string initialDataImagePath = "../../../../Streetcode.DAL/InitialData/images.json";
            string initialDataAudioPath = "../../../../Streetcode.DAL/InitialData/audios.json";
            string imageJson = File.ReadAllText(initialDataImagePath, Encoding.UTF8);
            string audiosJson = File.ReadAllText(initialDataAudioPath, Encoding.UTF8);
            var imgfromJson = JsonConvert.DeserializeObject<List<Image>>(imageJson) !;
            var audiosfromJson = JsonConvert.DeserializeObject<List<Audio>>(audiosJson) !;

            foreach (var img in imgfromJson)
            {
                string[] fullName = img.BlobName!.Split('.');
                this.SaveFileIfNotExist(img.Base64!, fullName[0], fullName[1]);
            }

            foreach (var audio in audiosfromJson)
            {
                string[] fullName = audio.BlobName!.Split('.');
                this.SaveFileIfNotExist(audio.Base64!, fullName[0], fullName[1]);
            }

            this.TestDbContext.Images.AddRange(imgfromJson);
            this.TestDbContext.Audios.AddRange(audiosfromJson);

            await this.TestDbContext.SaveChangesAsync();
        }

        private void SaveFileIfNotExist(string base64, string blobName, string extension)
        {
            string filePath = Path.Combine(this.BlobPath, blobName);
            if (!File.Exists(filePath))
            {
                this.BlobService.SaveFileInStorageBase64(base64, blobName, extension);
            }
        }
    }
}
