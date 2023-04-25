using Streetcode.BLL.Services.BlobStorageService;
using Streetcode.BLL.Services.Payment;

namespace Streetcode.WebApi.Extensions;

public static class ConfigureHostBuilderExtensions
{
    public static void ConfigureApplication(this ConfigureHostBuilder host)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";

        host.ConfigureAppConfiguration((_, config) =>
        {
            config.SetBasePath(Directory.GetCurrentDirectory());
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            config.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
            config.AddEnvironmentVariables("STREETCODE_");
        });
    }

    public static void ConfigureBlob(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.Configure<BlobEnvironmentVariables>(builder.Configuration.GetSection("Blob"));
    }

    public static void ConfigurePayment(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.Configure<PaymentEnvirovmentVariables>(builder.Configuration.GetSection("Payment"));
    }
}