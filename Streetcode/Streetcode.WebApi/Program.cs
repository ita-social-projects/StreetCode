using System.Globalization;
using System.Text.Json;
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

builder.Services.AddLocalization(option => option.ResourcesPath = "Resources");
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
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("ForgotPasswordRateLimit", httpContext =>
    {
        httpContext.Request.EnableBuffering();
        using var reader = new StreamReader(
            httpContext.Request.Body,
            encoding: System.Text.Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            leaveOpen: true);
        var body = reader.ReadToEndAsync().Result;
        httpContext.Request.Body.Position = 0;

        string? email = null;
        try
        {
            var json = JsonDocument.Parse(body);
            if (json.RootElement.TryGetProperty("email", out JsonElement emailElement))
            {
                email = emailElement.ToString();
            }
            else
            {
                Serilog.Log.Warning("Email field is missing in the request body for rate limiting in ForgotPasswordRateLimit policy.");
            }
        }
        catch (JsonException ex)
        {
            Serilog.Log.Warning(ex, "Failed to parse email from request body for rate limiting in ForgotPasswordRateLimit policy.");
        }

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: email ?? httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 1,
                QueueLimit = 0,
                Window = TimeSpan.FromSeconds(60)
            });
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

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsync("Too many requests.", cancellationToken: token);
    };
});

builder.Services.AddHttpClient();

var app = builder.Build();

app.UseForwardedHeaders();

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
if (app.Environment.EnvironmentName != "Production")
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
}
else
{
    app.UseHsts();
}

await app.ApplyMigrations();
if (!builder.Environment.EnvironmentName.Equals("IntegrationTests"))
{
    await app.SeedDataAsync();
}

app.AddCleanAudiosJob();
app.AddCleanImagesJob();
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

app.UseIpRateLimiting();
app.UseRateLimiter();

// BackgroundJob.Schedule<WebParsingUtils>(
//     wp => wp.ParseZipFileFromWebAsync(), TimeSpan.FromMinutes(1));
// RecurringJob.AddOrUpdate<WebParsingUtils>(
//     "ParseZipFileFromWebAsync",
//     wp => wp.ParseZipFileFromWebAsync(),
//     Cron.Monthly);

app.MapControllers();
app.UseMiddleware<CustomResponseCompressionMiddleware>();

app.Run();
public partial class Program
{
}