using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Streetcode.DAL.Persistence;

namespace Streetcode.WebApi.Extensions;

public static class WebApplicationExtensions
{
    public static async Task GenerateScript(
        this WebApplication app,
        string scriptsFolderPath = "./Streetcode.DAL/Persistence/ScriptsMigration/")
    {
        using var scope = app.Services.CreateScope();
        var logger = app.Services.GetRequiredService<ILogger<Program>>();

        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<StreetcodeDbContext>();

            var migrator = dbContext.GetInfrastructure().GetService<IMigrator>();

            var script = migrator.GenerateScript();

            var projRootDirectory = Directory.GetParent(Environment.CurrentDirectory)?.FullName!;
            var filePath = Path.Combine(projRootDirectory, scriptsFolderPath, "All_migrations_apply.sql");

            await File.WriteAllTextAsync(filePath, script);

            await dbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during migration and script generation");
        }
    }

    public static async Task ApplyMigrations(
        this WebApplication app,
        string scriptsFolderPath = "./Streetcode.DAL/Persistence/ScriptsMigration/")
    {
        using var scope = app.Services.CreateScope();
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        try
        {
            var streetcodeContext = scope.ServiceProvider.GetRequiredService<StreetcodeDbContext>();

            IDbContextTransaction transaction = null;
            try
            {
                var projRootDirectory = Directory.GetParent(Environment.CurrentDirectory)?.FullName!;

                var scriptFiles = Directory.GetFiles(Path.Combine(projRootDirectory, scriptsFolderPath));

                var filesContexts = await Task.WhenAll(scriptFiles.Select(file => File.ReadAllTextAsync(file)));
                transaction = streetcodeContext.Database.BeginTransaction();

                foreach (var singleSqlScript in filesContexts)
                {
                    await streetcodeContext.Database.ExecuteSqlRawAsync(singleSqlScript.Replace("GO", ""));
                }

                streetcodeContext.Database.CommitTransaction();
            }
            catch (Exception ex)
            {
                if (transaction != null)
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
