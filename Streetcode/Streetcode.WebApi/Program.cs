using Hangfire;
using Streetcode.WebApi.Extensions;
using Streetcode.WebApi.Utils;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureApplication();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddSwaggerServices();
builder.Services.AddCustomServices();

var app = builder.Build();
await app.MigrateAndSeedDbAsync();

if (app.Environment.EnvironmentName == "Local")
{
    builder.Configuration.AddUserSecrets<string>();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
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

// change Cron.Monthly to set another parsing interval from ukrposhta
RecurringJob.AddOrUpdate<WebParsingUtils>(
    wp => wp.ParseZipFileFromWebAsync(), Cron.Monthly);

app.MapControllers();

app.Run();
