using System.Globalization;
using AspNetCoreRateLimit;
using Hangfire;
using Streetcode.WebApi.Extensions;
using Microsoft.AspNetCore.Localization;
using Streetcode.BLL.Services.Hangfire;
using Microsoft.AspNetCore.HttpOverrides;
using Streetcode.BLL.Middleware;
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
builder.Services.ConfigureRequestResponseMiddlewareOptions(builder);
builder.Services.ConfigureRateLimitMiddleware(builder);
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

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

app.MapControllers();

app.Run();
public partial class Program
{
}