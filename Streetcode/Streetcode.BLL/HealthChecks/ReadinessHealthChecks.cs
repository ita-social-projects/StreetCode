using System.Net;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Streetcode.BLL.Interfaces.BlobStorage;
using Microsoft.Extensions.Options;

namespace Streetcode.BLL.HealthChecks
{
    public class ReadinessHealthChecks : IHealthCheck
    {
        private readonly string _databaseProblem;
        private readonly string _blobStorageProblem;
        private readonly string _apiProblem;
        private readonly string _healthyReadiness;
        private readonly HealthChecksOptions _options;
        private readonly IBlobService _blobService;

        public ReadinessHealthChecks(IOptions<HealthChecksOptions> options, IBlobService blobService)
        {
            _options = options.Value;
            _blobService = blobService;
            _databaseProblem = $"{Environment.NewLine}Database is not available";
            _blobStorageProblem = $"{Environment.NewLine}Blob storage is not available";
            _apiProblem = $"{Environment.NewLine}API is not available";
            _healthyReadiness = $"{Environment.NewLine}The database is available for use." +
                $"{Environment.NewLine}The blob storage is accessible and ready for data storage." +
                $"{Environment.NewLine}The API is up and running, ready to handle requests.";
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return VerifyReadinessAsync();
        }

        private async Task<HealthCheckResult> VerifyReadinessAsync()
        {
            bool isDatabaseAvailable = await CheckDatabaseAvailability();
            bool isBlobStorageAvailable = await CheckBlobStorageAvailability();
            bool isApiAvailable = await CheckApiAvailability();
            string description = string.Empty;
            if (!isDatabaseAvailable)
            {
                description += _databaseProblem;
            }

            if (!isBlobStorageAvailable)
            {
                description += _blobStorageProblem;
            }

            if (!isApiAvailable)
            {
                description += _apiProblem;
            }

            if (isDatabaseAvailable && isApiAvailable && isBlobStorageAvailable)
            {
                description = _healthyReadiness;
                return HealthCheckResult.Healthy(description);
            }
            else
            {
                return HealthCheckResult.Unhealthy(description);
            }
        }

        private async Task<bool> CheckDatabaseAvailability()
        {
            try
            {
                string connectionString = _options.DefaultConnection;
                using (SqlConnection connection =
                    new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> CheckBlobStorageAvailability()
        {
            const uint MIN_BYTE_SIZE = 1024;
            string relativePath = _options.BlobStoragePath;
            string absolutePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, relativePath));
            byte[] testData = new byte[MIN_BYTE_SIZE];
            new Random().NextBytes(testData);
            string base64 = Convert.ToBase64String(testData);
            string title = DateTime.Now.ToString();
            string extension = "mp3";
            string hashBlobStorageName = _blobService.SaveFileInStorage(base64, title, extension);
            string fileName = $"{hashBlobStorageName}.{extension}";
            string filePath = Path.Combine(absolutePath, fileName);

            try
            {
                if (!Directory.Exists(absolutePath) && string.IsNullOrEmpty(hashBlobStorageName) && filePath == Path.GetFileName(filePath))
                {
                    return false;
                }

                _blobService.DeleteFileInStorage(fileName);
                if (File.Exists(filePath))
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<bool> CheckApiAvailability()
        {
            string url = _options.GlobalUrl;
            const string ENDPOINT = "/api/Audio/GetAll";
            url = url + ENDPOINT;
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("");
            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}