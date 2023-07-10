using Microsoft.Extensions.Diagnostics.HealthChecks;
using Streetcode.BLL.HealthChecks.MemoryMetrics;

namespace Streetcode.BLL.HealthChecks
{
    public class LivenessHealthChecks : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var client = new MemoryMetricsClient();
            var metrics = client.GetMetrics();
            var percentUsed = 100 * metrics.Used / metrics.Total;
            const string HEALTHY = "System memory used is up to 80%";
            const string DEGRADED = "System memory used is between 80% - 90%";
            const string UNHEALTHY = "System memory used is over 90%";

            var status = HealthStatus.Healthy;
            string healthDescription = HEALTHY;

            if (percentUsed >= 80 && percentUsed <= 90)
            {
                status = HealthStatus.Degraded;
                healthDescription = DEGRADED;
            }

            if (percentUsed > 90)
            {
                status = HealthStatus.Unhealthy;
                healthDescription = UNHEALTHY;
            }

            var data = new Dictionary<string, object>();
            data.Add(nameof(metrics.Total), metrics.Total);
            data.Add(nameof(metrics.Used), metrics.Used);
            data.Add(nameof(metrics.Free), metrics.Free);
            data.Add(nameof(metrics.Duration), metrics.Duration);

            var result = new HealthCheckResult(status, healthDescription, null, data);

            return await Task.FromResult(result);
        }
    }
}