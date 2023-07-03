﻿using Serilog;
using Streetcode.BLL.Middleware;
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
}