using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Streetcode.DAL.Persistence;

namespace Streetcode.WebApi.Extensions;

public static class WebApplicationExtensions
{
    public static async Task MigrateAndSeedDbAsync(
        this WebApplication app,
        string scriptsFolderPath = "./Streetcode.DAL/Persistence/Scripts/")
    {
        using var scope = app.Services.CreateScope();
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        try
        {
            var streetcodeContext = scope.ServiceProvider.GetRequiredService<StreetcodeDbContext>();

            await streetcodeContext.Database.MigrateAsync();
            IDbContextTransaction transaction = null;
            try
            {
                var projRootDirectory = Directory.GetParent(Environment.CurrentDirectory)?.FullName!;

                var scriptFiles = Directory.GetFiles($"{projRootDirectory}/{scriptsFolderPath}");

                var filesContexts = await Task.WhenAll(scriptFiles.Select(file => File.ReadAllTextAsync(file)));
                transaction = streetcodeContext.Database.BeginTransaction();
                foreach (var task in filesContexts)
                {
                    await streetcodeContext.Database.ExecuteSqlRawAsync(task);
                }

                streetcodeContext.Database.CommitTransaction();
            }
            catch(Exception ex)
            {
                if(transaction != null)
                {
                    streetcodeContext.Database.RollbackTransaction();
                    logger.LogError(ex, "An error occured during adding relations");
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occured during startup migration");
        }
    }
}