using Hangfire;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using Streetcode.WebApi.Extensions;
using Streetcode.WebApi.Utils;
using Streetcode.BLL.Services.BlobStorageService;
using FluentResults;
using AspNetCoreRateLimit;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureApplication();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddSwaggerServices();
builder.Services.AddCustomServices();
builder.Services.ConfigureBlob(builder);
builder.Services.ConfigurePayment(builder);
builder.Services.ConfigureInstagram(builder);

builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddInMemoryRateLimiting();

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
app.UseIpRateLimiting();
app.UseRouting();

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
