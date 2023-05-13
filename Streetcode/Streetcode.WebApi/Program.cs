using Hangfire;
using Streetcode.BLL.Middleware;
using Streetcode.WebApi.Extensions;
using Streetcode.WebApi.Utils;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureApplication();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddSwaggerServices();
builder.Services.AddCustomServices();
builder.Services.ConfigureBlob(builder);
builder.Services.ConfigurePayment(builder);

var app = builder.Build();

// await app.ApplyMigrations();
await app.MigrateAndSeedDbAsync();

if (app.Environment.EnvironmentName == "Local")
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
}
else
{
    app.UseHsts();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseMiddleware<ApiRequestResponseMiddleware>();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard();

// change Cron.Monthly to set another parsing interval from ukrposhta
/*RecurringJob.AddOrUpdate<WebParsingUtils>(
    wp => wp.ParseZipFileFromWebAsync(), Cron.Monthly);*/

/*BackgroundJob.Schedule<WebParsingUtils>(
    wp => wp.ParseZipFileFromWebAsync(), TimeSpan.FromMinutes(1));
RecurringJob.AddOrUpdate<WebParsingUtils>(
    wp => wp.ParseZipFileFromWebAsync(), Cron.Monthly);*/

app.MapControllers();

app.Run();