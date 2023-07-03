using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Services.BlobStorageService;

namespace Streetcode.BLL.HealthChecks
{
    public class ReadinessHealthChecks : IHealthCheck
    {
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
            
            string description = "";
            
            if(!isDatabaseAvailable)
            {
                description += "\nDatabase is not available";
            }

            if (!isBlobStorageAvailable)
            {
                description += "\nBlobstorage is not available";
            }

            if (!isApiAvailable) 
            {
                description += "\nAPI is not available";
            }

            if (isDatabaseAvailable && isApiAvailable && isBlobStorageAvailable)
            {
                return HealthCheckResult.Healthy("All dependencies are available");
            }
            else
            {
                return HealthCheckResult.Unhealthy(description);
            }
        }

        private static async Task<bool> CheckDatabaseAvailability()
        {
            try
            {
                using (SqlConnection connection =
                    new SqlConnection("Server=localhost;Database=StreetcodeDb;Integrated Security=True;MultipleActiveResultSets=true"))
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

                string testFilePath = Path.Combine(absolutePath, "test.txt");
                File.WriteAllText(testFilePath, "test");
                File.Delete(testFilePath);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static async Task<bool> CheckApiAvailability()
        {
            string urlCheck = "https://localhost:5001/api/Partners/GetAll";
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync(urlCheck);

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
