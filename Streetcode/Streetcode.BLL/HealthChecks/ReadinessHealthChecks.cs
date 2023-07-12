using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;

namespace Streetcode.BLL.HealthChecks
{
    public class ReadinessHealthChecks : IHealthCheck
    {
        private readonly string _databaseProblem;
        private readonly string _blobStorageProblem;
        private readonly string _apiProblem;
        private readonly string _healthyReadiness;
        private readonly IConfiguration _configuration;

        public ReadinessHealthChecks(IConfiguration configuration)
        {
            _configuration = configuration;
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
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
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
            string relativePath = _configuration["Blob:BlobStorePath"];
            string absolutePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, relativePath));
            try
            {
                if (!Directory.Exists(absolutePath))
                {
                    return false;
                }

                string checkFilePath = Path.Combine(absolutePath, "check.txt");
                await File.WriteAllTextAsync(checkFilePath, "check");
                File.Delete(checkFilePath);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<bool> CheckApiAvailability()
        {
            const string ENDPOINT = "api/Audio/GetAll";
            string apiUrl = _configuration["Jwt:Issuer"];
            string apiCheck = apiUrl + ENDPOINT;
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync(apiCheck);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}