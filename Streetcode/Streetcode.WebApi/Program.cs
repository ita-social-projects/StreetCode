using System.Resources;
using System.Reflection;
using System.Globalization;
using Hangfire;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using Streetcode.WebApi.Extensions;
using Streetcode.WebApi.Utils;
using Streetcode.BLL.Services.BlobStorageService;

var builder = WebApplication.CreateBuilder(args);
var supportedCultures = new[] { new CultureInfo("en-US"), new CultureInfo("uk-UA") };
var requestLocalizationOptions = new RequestLocalizationOptions
{
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};
builder.Host.ConfigureApplication();

builder.Services.AddLocalization();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddSwaggerServices();
builder.Services.AddCustomServices();
builder.Services.ConfigureBlob(builder);
builder.Services.ConfigurePayment(builder);
builder.Services.ConfigureInstagram(builder);

//string baseName = "WebApi.Controllers.Streetcode.StreetcodeController.";
//builder.Services.AddSingleton(new ResourceManager(baseName, Assembly.GetExecutingAssembly()));
var app = builder.Build();
app.UseRequestLocalization(requestLocalizationOptions);
if (app.Environment.EnvironmentName == "Local")
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
    await app.ApplyMigrations();
    await app.SeedDataAsync();
}
else
{
    app.UseHsts();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard();

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
