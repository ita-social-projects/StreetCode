using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Streetcode.BLL.Interfaces.Audio;
using Streetcode.BLL.Interfaces.Image;

namespace Streetcode.WebApi.Extensions;

public static class RecurringJobExtensions
{
    public static void AddCleanAudiosJob(this WebApplication app)
    {
        var serviceScopeFactory = app.Services.GetService<IServiceScopeFactory>();
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var recurringJobManager = scope.ServiceProvider.GetService<IRecurringJobManager>();
            var audioService = scope.ServiceProvider.GetService<IAudioService>();

            recurringJobManager.AddOrUpdate(
                "Clean audio that are not used in streetcodes",
                () => audioService.CleanUnusedAudiosAsync(),
                app.Configuration.GetSection("RecurringJobs")["AudioCleaningFrequency"],
                TimeZoneInfo.Utc);
        }
    }

    public static void AddCleanImagesJob(this WebApplication app)
    {
        var serviceScopeFactory = app.Services.GetService<IServiceScopeFactory>();
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var recurringJobManager = scope.ServiceProvider.GetService<IRecurringJobManager>();
            var imageService = scope.ServiceProvider.GetService<IImageService>();

            recurringJobManager.AddOrUpdate(
                "Clean images that are not used",
                () => imageService.CleanUnusedImagesAsync(),
                app.Configuration.GetSection("RecurringJobs")["ImageCleaningFrequency"],
                TimeZoneInfo.Utc);
        }
    }
}