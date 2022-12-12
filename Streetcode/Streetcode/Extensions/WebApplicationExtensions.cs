using System;
using System.Threading.Tasks;
using EFTask.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EFTask.Extensions;

public static class WebApplicationExtensions
{
    public static async Task MigrateToDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        try
        {
            var productContext = scope.ServiceProvider.GetRequiredService<StreetcodeDbContext>();
            await productContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occured during startup migration");
        }
    }
}