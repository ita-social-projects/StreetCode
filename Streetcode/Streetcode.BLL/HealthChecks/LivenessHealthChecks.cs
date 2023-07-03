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
            var description = "System memory used is up to 80%";

            var status = HealthStatus.Healthy;

            if (percentUsed >= 80 && percentUsed <= 90)
            {
                status = HealthStatus.Degraded;
                description = "System memory used is between 80% - 90%";
            }

            if (percentUsed > 90)
            {
                status = HealthStatus.Unhealthy;
                description = "System memory used is over 90%";
            }

            var data = new Dictionary<string, object>();
            data.Add("Total", metrics.Total);
            data.Add("Used", metrics.Used);
            data.Add("Free", metrics.Free);
            data.Add("Duration", metrics.Duration);

            var result = new HealthCheckResult(status, description, null, data);

            return await Task.FromResult(result);
        }
    }
}
