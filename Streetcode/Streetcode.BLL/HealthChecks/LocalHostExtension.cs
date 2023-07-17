using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Streetcode.BLL.HealthChecks
{
    public static class LocalHostExtension
    {
        public static void ConfigureLocalhostOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LocalhostOptions>(configuration.GetSection("MySettings"));
        }
    }
}
