using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Persistence;

namespace Streetcode.WebApi.Extensions;

public static class WebApplicationExtensions
{
    public static async Task ApplyMigrations(this WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        try
        {
            var streetcodeContext = app.Services.GetRequiredService<StreetcodeDbContext>();

            var pendingMigrations = await streetcodeContext.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                logger.LogInformation("Pending migrations: {PendingMigrations}", string.Join(", ", pendingMigrations));
                var appliedMigrationsBefore = await streetcodeContext.Database.GetAppliedMigrationsAsync();
                await streetcodeContext.Database.MigrateAsync();
                logger.LogInformation("Migrations applied successfully.");
                var appliedMigrationsAfter = await streetcodeContext.Database.GetAppliedMigrationsAsync();
                var newlyAppliedMigrations = appliedMigrationsAfter.Except(appliedMigrationsBefore);

                if (newlyAppliedMigrations.Any())
                {
                    logger.LogInformation("Newly applied migrations: {NewlyAppliedMigrations}", string.Join(", ", newlyAppliedMigrations));
                }
                else
                {
                    logger.LogInformation("No new migrations were applied.");
                }
            }
            else
            {
                logger.LogInformation("No pending migrations.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during startup migration");
        }
    }
}
