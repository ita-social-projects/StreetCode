namespace Streetcode.XIntegrationTest.ControllerTests.Utils
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.Configuration;
    using Streetcode.WebApi.Extensions;
    using System;
    using System.Drawing;

    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
        where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, builder) =>
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";
                builder.SetBasePath(Directory.GetCurrentDirectory());
                builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                builder.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
                builder.AddEnvironmentVariables("STREETCODE_");
            });
        }
    }
}
