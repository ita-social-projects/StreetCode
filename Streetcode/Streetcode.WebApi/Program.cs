using System.Globalization;
using AspNetCoreRateLimit;
using Hangfire;
using Streetcode.BLL.Services.BlobStorageService;
using Streetcode.WebApi.Extensions;
using Streetcode.WebApi.Utils;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureApplication();

builder.Services.AddLocalization(option => option.ResourcesPath = "Resources");
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddSwaggerServices();
builder.Services.AddCustomServices();
builder.Services.ConfigureBlob(builder);
builder.Services.ConfigurePayment(builder);
builder.Services.ConfigureInstagram(builder);
builder.Services.ConfigureSerilog(builder);
builder.Services.ConfigureRateLimitMiddleware(builder);
var app = builder.Build();
var supportedCulture = new[]
{
    new CultureInfo("en-US"),
    new CultureInfo("uk-UA")
};
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("uk-UA"),
    SupportedCultures = supportedCulture,
    SupportedUICultures = supportedCulture,
    ApplyCurrentCultureToResponseHeaders = true
});
if (app.Environment.EnvironmentName == "Local" || app.Environment.EnvironmentName == "Staging")
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
}
else
{
    app.UseHsts();
}

await app.ApplyMigrations();

app.UseCors();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard("/dash");

app.UseIpRateLimiting();

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
public partial class Program
{
}