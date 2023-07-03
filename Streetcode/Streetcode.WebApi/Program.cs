using Hangfire;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using Streetcode.WebApi.Extensions;
using Streetcode.WebApi.Utils;
using Streetcode.BLL.Services.BlobStorageService;
using Streetcode.BLL.Middleware;
using Streetcode.BLL.HealthChecks;
using Streetcode.BLL.Services.Logging;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using k8s.Models;
using Newtonsoft.Json;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureApplication();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddSwaggerServices();
builder.Services.AddCustomServices();
builder.Services.ConfigureBlob(builder);
builder.Services.ConfigurePayment(builder);
builder.Services.ConfigureInstagram(builder);
builder.Services.ConfigureSerilog(builder);

var app = builder.Build();

if (app.Environment.EnvironmentName == "Local")
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
    await app.ApplyMigrations();
/*    await app.SeedDataAsync();*/
}
else
{
    app.UseHsts();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseRequestResponseMiddleware();
app.UseRouting();

app.MapHealthChecksUI();
app.MapHealthChecks("/healthz", new()
{
    Predicate = check => check.Name == "StartupProbe",
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/health/ready", new()
{
    Predicate = check => check.Name == "ReadinessProbe",
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/health/live", new()
{
    Predicate = check => check.Name == "LivenessProbe",
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/dash");

if (app.Environment.EnvironmentName != "Local")
{
    BackgroundJob.Schedule<WebParsingUtils>(
      wp => wp.ParseZipFileFromWebAsync(), TimeSpan.FromMinutes(1));
    RecurringJob.AddOrUpdate<WebParsingUtils>(
      wp => wp.ParseZipFileFromWebAsync(), Cron.Monthly);
    RecurringJob.AddOrUpdate<BlobService>(
        b => b.CleanBlobStorage(), Cron.Monthly);
}

app.MapControllers();

app.Run();
