using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Persistence;

namespace Streetcode.WebApi.Extensions;

public static class WebApplicationExtensions
{
    public static async Task MigrateAndSeedDbAsync(
        this WebApplication app,
        string scriptsFolderPath = "./Streetcode.DAL/Persistence/Scripts/")
    {
        using var scope = app.Services.CreateScope();
        try
        {
            var streetcodeContext = scope.ServiceProvider.GetRequiredService<StreetcodeDbContext>();

            // await streetcodeContext.Database.EnsureDeletedAsync();

            await streetcodeContext.Database.MigrateAsync();

            // var projRootDirectory = Directory.GetParent(Environment.CurrentDirectory)?.FullName!;

            // var scriptFiles = Directory.GetFiles($"{projRootDirectory}/{scriptsFolderPath}");

            // var filesContexts = await Task.WhenAll(scriptFiles.Select(file => File.ReadAllTextAsync(file)));

            // foreach (var task in filesContexts)
            // {
            //    await streetcodeContext.Database.ExecuteSqlRawAsync(task);
            // }
        }
        catch (Exception ex)
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occured during startup migration");
        }
    }
}