using System.IO.Compression;
using System.Runtime;
using Microsoft.Extensions.Configuration;
using Serilog;
using Streetcode.BLL.Middleware;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.ResponseCompression;
using Streetcode.BLL.Services.BlobStorageService;
using Streetcode.BLL.Services.Instagram;
using Streetcode.BLL.Services.Payment;

namespace Streetcode.WebApi.Extensions;

public static class ConfigureHostBuilderExtensions
{
    public static void ConfigureApplication(this ConfigureHostBuilder host)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";

        host.ConfigureAppConfiguration((_, config) =>
        {
            config.ConfigureCustom(environment);
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

    public static void ConfigureInstagram(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.Configure<InstagramEnvirovmentVariables>(builder.Configuration.GetSection("Instagram"));
    }

    public static void ConfigureSerilog(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var filterExpression = builder.Configuration["Serilog:Filter:ByExcluding"];

        builder.Host.UseSerilog((ctx, services, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(builder.Configuration);

            if (!string.IsNullOrEmpty(filterExpression))
            {
                loggerConfiguration.Filter.ByExcluding(filterExpression);
            }
        });
    }

    public static void ConfigureRequestResponseMiddlewareOptions(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.Configure<RequestResponseMiddlewareOptions>(builder.Configuration.GetSection("RequestResponseMiddlewareOptions"));
    }

    public static void ConfigureRateLimitMiddleware(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        builder.Services.AddInMemoryRateLimiting();
        services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    }

    public static void ConfigureResponseCompressingMiddleware(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<GzipCompressionProvider>();
        });
        services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.SmallestSize;
        });
    }
}