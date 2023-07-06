using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using HealthChecks.UI.Core;

namespace Streetcode.BLL.HealthChecks
{
    public static class HealthChecksExtension
    {
        public static IEndpointRouteBuilder UseHealthChecks(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHealthChecks("/healthz", new HealthCheckOptions
            {
                Predicate = check => check.Name == "StartupProbe",
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = check => check.Name == "ReadinessProbe",
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = check => check.Name == "LivenessProbe",
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            return endpoints;
        }
    }
}
