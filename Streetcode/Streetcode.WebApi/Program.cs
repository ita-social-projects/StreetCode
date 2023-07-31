using Hangfire;
using Streetcode.BLL.Services.BlobStorageService;
using Streetcode.WebApi.Extensions;
using Streetcode.WebApi.Utils;
using Streetcode.BLL.Middleware;
using Streetcode.BLL.HealthChecks;
using Streetcode.BLL.Services.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureApplication();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddSwaggerServices();
builder.Services.AddCustomServices();
builder.Services.ConfigureBlob(builder);
builder.Services.ConfigurePayment(builder);
builder.Services.ConfigureInstagram(builder);
builder.Services.ConfigureSerilog(builder);
builder.Services.ConfigureMiddleware(builder);
builder.Services.ConfigureHealthCheck(builder);
var app = builder.Build();

if (app.Environment.EnvironmentName.ToLower() == "local")
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
    await app.ApplyMigrations();
    /* await app.SeedDataAsync(); */
}
else
{
    app.UseHsts();
}

var appUrl = builder.Configuration.GetSection("ApplicationUrls").Get<string[]>();
app.Urls.Add(appUrl[1]);
app.UseCors();
app.UseHttpsRedirection();
app.UseRequestResponseMiddleware();
app.UseRouting();

app.MapHealthChecksUI();
app.UseHealthChecks();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/dash");

if (app.Environment.EnvironmentName.ToLower() != "local")
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