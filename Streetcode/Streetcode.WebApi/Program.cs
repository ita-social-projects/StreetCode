using System.Globalization;
using System.Threading.RateLimiting;
using AspNetCoreRateLimit;
using Hangfire;
using Streetcode.WebApi.Extensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.HttpOverrides;
using Streetcode.WebApi.Hangfire;
using Streetcode.WebApi.Middleware;
using Streetcode.WebApi.Middleware.ApiRequestResponseMiddleware;
using Streetcode.WebApi.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureApplication(builder);

// Localization
builder.Services.AddLocalization(option => option.ResourcesPath = "Resources");

// Add application services
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddSwaggerServices();
builder.Services.AddCustomServices();
builder.Services.ConfigureBlob(builder);
builder.Services.ConfigurePayment(builder);
builder.Services.ConfigureInstagram(builder);
builder.Services.ConfigureSerilog(builder);
builder.Services.ConfigureRequestResponseMiddlewareOptions(builder);
builder.Services.ConfigureRateLimitMiddleware(builder);
builder.Services.ConfigureResponseCompressingMiddleware(builder);

// Configure forwarded headers
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

// ✅ **Enhanced Rate Limiting Configuration**
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("api", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 100; // General API: 100 requests per minute
    });

    options.AddFixedWindowLimiter("registration", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(10);
        opt.PermitLimit = 5; // Registration: 5 attempts per 10 minutes
    });

    options.AddFixedWindowLimiter("token-refresh", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(5);
        opt.PermitLimit = 10; // Token Refresh: 10 attempts per 5 minutes
    });

    options.AddPolicy("EmailRateLimit", context => RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
        factory: _ => new FixedWindowRateLimiterOptions
        {
            AutoReplenishment = true,
            PermitLimit = 3,
            QueueLimit = 0,
            Window = TimeSpan.FromMinutes(5)
        }));

    // Global Rate Limiting Handler
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsync("Too many requests.", cancellationToken: token);
    };
});

// Add HttpClient
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseForwardedHeaders();

// ✅ **Enhanced Localization**
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

// ✅ **Swagger only for non-production**
if (app.Environment.EnvironmentName != "Production")
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
}
else
{
    app.UseHsts();
}

// Apply migrations
await app.ApplyMigrations();

// Hangfire jobs
app.AddCleanAudiosJob();
app.AddCleanImagesJob();

// ✅ **Security & Middleware**
app.UseCors();
app.UseHttpsRedirection();
app.UseRequestResponseMiddleware();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireDashboardAuthorizationFilter() }
});

// ✅ **Enable Rate Limiting & IP Rate Limiting**
app.UseIpRateLimiting();
app.UseRateLimiter();

// ✅ **Background Jobs**
BackgroundJob.Schedule<WebParsingUtils>(
    wp => wp.ParseZipFileFromWebAsync(), TimeSpan.FromMinutes(1));
RecurringJob.AddOrUpdate<WebParsingUtils>(
    "ParseZipFileFromWebAsync",
    wp => wp.ParseZipFileFromWebAsync(),
    Cron.Monthly);

// ✅ **Ensure All Controllers Are Mapped**
app.MapControllers();

// ✅ **Custom Response Compression Middleware**
app.UseMiddleware<CustomResponseCompressionMiddleware>();

app.Run();

public partial class Program
{
}
