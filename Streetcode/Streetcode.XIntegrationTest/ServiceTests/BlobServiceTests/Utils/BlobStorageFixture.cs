﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Streetcode.BLL.Services.BlobStorageService;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Realizations.Base;
using Streetcode.XIntegrationTest.Base;
using System.Text;

namespace Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.Utils
{
    public class BlobStorageFixture : IntegrationTestBase
    {
        public readonly BlobService blobService;
        private readonly IOptions<BlobEnvironmentVariables> environmentVariables;
        public readonly string blobPath;
        private readonly string blobKey;
        public StreetcodeDbContext TestDbContext { get; private set; }

        public BlobStorageFixture()
        {
            environmentVariables = Options.Create(new BlobEnvironmentVariables
            {
                BlobStoreKey = Configuration.GetValue<string>("Blob:BlobStoreKey"),
                BlobStorePath = Configuration.GetValue<string>("Blob:BlobStorePath")
            });
            blobPath = environmentVariables.Value.BlobStorePath;
            blobKey = environmentVariables.Value.BlobStoreKey;

            TestDbContext = TestDBFixture.CreateContext(Configuration.GetConnectionString("DefaultConnection"));
            RepositoryWrapper repo = new RepositoryWrapper(TestDbContext);
            blobService = new BlobService(environmentVariables, repo);
            Directory.CreateDirectory(blobPath);
        }

        public void Seeding(string givenBlobName)
        {
            string testDataImagePath = "../../../ServiceTests/BlobServiceTests/Utils/testData.json";
            string imageJson = File.ReadAllText(testDataImagePath, Encoding.UTF8);
            Image imgfromJson = JsonConvert.DeserializeObject<Image>(imageJson);
            imgfromJson.BlobName = givenBlobName;
            SaveFileIfNotExist(imgfromJson.Base64, imgfromJson.BlobName, imgfromJson.MimeType.Split('/')[1]);
        }

        public async Task DbAndStorageSeeding()
        {
            string initialDataImagePath = "../../../../Streetcode.DAL/InitialData/images.json";
            string initialDataAudioPath = "../../../../Streetcode.DAL/InitialData/audios.json";
            string imageJson = File.ReadAllText(initialDataImagePath, Encoding.UTF8);
            string audiosJson = File.ReadAllText(initialDataAudioPath, Encoding.UTF8);
            var imgfromJson = JsonConvert.DeserializeObject<List<Image>>(imageJson);
            var audiosfromJson = JsonConvert.DeserializeObject<List<Audio>>(audiosJson);

            foreach (var img in imgfromJson)
            {
                string[] fullName = img.BlobName.Split('.');
                SaveFileIfNotExist(img.Base64, fullName[0], fullName[1]);
            }

            foreach (var audio in audiosfromJson)
            {
                string[] fullName = audio.BlobName.Split('.');
                SaveFileIfNotExist(audio.Base64, fullName[0], fullName[1]);
            }

            TestDbContext.Images.AddRange(imgfromJson);
            TestDbContext.Audios.AddRange(audiosfromJson);

            await TestDbContext.SaveChangesAsync();
        }

        private void SaveFileIfNotExist(string base64, string blobName, string extension)
        {
            string filePath = Path.Combine(blobPath, blobName);
            if (!File.Exists(filePath))
            {
                blobService.SaveFileInStorageBase64(base64, blobName, extension);
            }
        }
    }
}
