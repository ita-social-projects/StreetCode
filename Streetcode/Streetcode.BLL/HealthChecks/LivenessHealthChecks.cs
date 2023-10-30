using Microsoft.Extensions.Diagnostics.HealthChecks;
using Streetcode.BLL.HealthChecks.MemoryMetrics;
using Streetcode.BLL.Interfaces.Logging;

namespace Streetcode.BLL.HealthChecks
{
    public class LivenessHealthChecks : IHealthCheck
    {
        private readonly ILoggerService _loggerService;

        public LivenessHealthChecks(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            const uint PERCENTAGE_MIN = 80;
            const uint PERCENTAGE_MAX = 90;
            var client = new MemoryMetricsClient(_loggerService);
            var metrics = client.GetMetrics();
            var percentUsed = 100 * metrics.Used / metrics.Total;
            string healthy = $"System memory used is up to {PERCENTAGE_MIN}%";
            string degraded = $"System memory used is between {PERCENTAGE_MIN}% - {PERCENTAGE_MAX}%";
            string unhealthy = $"System memory used is over {PERCENTAGE_MAX}%";

            var status = HealthStatus.Healthy;
            string healthDescription = healthy;

            if (percentUsed >= PERCENTAGE_MIN && percentUsed <= PERCENTAGE_MAX)
            {
                status = HealthStatus.Degraded;
                healthDescription = degraded;
            }

            if (percentUsed > PERCENTAGE_MAX)
            {
                status = HealthStatus.Unhealthy;
                healthDescription = unhealthy;
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