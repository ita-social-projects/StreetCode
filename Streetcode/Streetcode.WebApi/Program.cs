using Streetcode.WebApi.Extensions;
using Streetcode.WebApi.Utils;
using Streetcode.DAL.Repositories.Realizations.Base;
using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureApplication();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddSwaggerServices();
builder.Services.AddCustomServices();

static void ExecuteMethod(object state)
{
    (string excelPath, string csvPath) = ((string, string))state;
    var columnsToExtract = new[] { 2, 5, 6 }; // Column indices for B, E, and F
}

var app = builder.Build();
await app.MigrateAndSeedDbAsync();
/*
await using var timer = new Timer(
    WebParsingUtils.ParseZipFileFromWeb,
    null,
    TimeSpan.Zero,
    TimeSpan.FromHours(12));
*/

using var scope = app.Services.CreateScope();
var streetcodeContext = scope.ServiceProvider.GetRequiredService<StreetcodeDbContext>();
WebParsingUtils utils = new WebParsingUtils(new RepositoryWrapper(streetcodeContext));
utils.SaveToponymToDb();

if (app.Environment.EnvironmentName == "Local")
{
    // builder.Configuration.AddUserSecrets<string>();

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

app.MapControllers();

app.Run();