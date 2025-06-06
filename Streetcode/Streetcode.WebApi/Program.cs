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