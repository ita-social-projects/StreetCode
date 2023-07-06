using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;

namespace Streetcode.BLL.HealthChecks
{
    public class ReadinessHealthChecks : IHealthCheck
    {
        private readonly IConfiguration _configuration;
        public ReadinessHealthChecks(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            // Readiness Probe ("/health/ready")
            return VerifyReadinessAsync();
        }

        private async Task<HealthCheckResult> VerifyReadinessAsync()
        {
            bool isDatabaseAvailable = await CheckDatabaseAvailability();
            bool isBlobStorageAvailable = await CheckBlobStorageAvailability();
            bool isApiAvailable = await CheckApiAvailability();
            const string DATABASE_PROBLEM = "\nDatabase is not available";
            const string BLOBSTORAGE_PROBLEM = "\nBlobstorage is not available";
            const string API_PROBLEM = "\nAPI is not available";
            const string HEALTHY_READINESS = "\nThe database is available for use." +
                "\nThe blob storage is accessible and ready for data storage." +
                "\nThe API is up and running, ready to handle requests.";
            string description = string.Empty;
            if(!isDatabaseAvailable)
            {
                description += DATABASE_PROBLEM;
            }

            if (!isBlobStorageAvailable)
            {
                description += BLOBSTORAGE_PROBLEM;
            }

            if (!isApiAvailable)
            {
                description += API_PROBLEM;
            }

            if (isDatabaseAvailable && isApiAvailable && isBlobStorageAvailable)
            {
                description = HEALTHY_READINESS;
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

        private static async Task<bool> CheckBlobStorageAvailability()
        {
            string relativePath = @"..\..\BlobStorage";
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
            const string ENDPOINT = "api/Partners/GetAll";
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
