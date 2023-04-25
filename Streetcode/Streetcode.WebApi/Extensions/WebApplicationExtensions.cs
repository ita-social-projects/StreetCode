    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using Streetcode.DAL.Persistence;

    namespace Streetcode.WebApi.Extensions;

    public static class WebApplicationExtensions
    {
        public static async Task MigrateAndSeedDbAsync(
            this WebApplication app,
            string scriptsFolderPath = "./Streetcode.DAL/Persistence/ScriptsSeeding/")
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

                    var scriptFiles = Directory.GetFiles(Path.Combine(projRootDirectory, scriptsFolderPath));


                var filesContexts = await Task.WhenAll(scriptFiles.Select(file => File.ReadAllTextAsync(file)));

                foreach (var task in filesContexts)
                    var filesContexts = await Task.WhenAll(scriptFiles.Select(file => File.ReadAllTextAsync(file)));
                    transaction = streetcodeContext.Database.BeginTransaction();
                    foreach (var singleSqlScript in filesContexts)
                    {
                        await streetcodeContext.Database.ExecuteSqlRawAsync(singleSqlScript);
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
                if(transaction != null)
                {
                    logger.LogError(ex, "An error occured during adding relations");
                }
                logger.LogError(ex, "An error occured during startup migration");
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