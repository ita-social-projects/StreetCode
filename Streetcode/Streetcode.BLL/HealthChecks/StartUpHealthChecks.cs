using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Streetcode.BLL.HealthChecks
{
    public class StartUpHealthChecks : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var startup_probe = HealthCheckResult.Healthy("App started");
            return Task.FromResult(startup_probe);
        }
    }
}