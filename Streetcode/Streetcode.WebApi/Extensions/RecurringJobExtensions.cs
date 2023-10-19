using Hangfire;
using Streetcode.BLL.Interfaces.Audio;
using Streetcode.BLL.Interfaces.Image;

namespace Streetcode.WebApi.Extensions;

public static class RecurringJobExtensions
{
    public static void AddCleanAudiosJob(this WebApplication app)
    {
        var recurringJobManager = app.Services.GetService<IRecurringJobManager>();

        recurringJobManager.AddOrUpdate(
            "Clean audio that are not used in streetcodes",
            () => app.Services.GetService<IAudioService>().CleanUnusedAudiosAsync(),
            app.Configuration.GetSection("RecurringJobs")["AudioCleaningFrequency"],
            TimeZoneInfo.Utc);
    }

    public static void AddCleanImagesJob(this WebApplication app)
    {
        var recurringJobManager = app.Services.GetService<IRecurringJobManager>();

        recurringJobManager.AddOrUpdate(
            "Clean images that are not used",
            () => app.Services.GetService<IImageService>().CleanUnusedImagesAsync(),
            app.Configuration.GetSection("RecurringJobs")["ImageCleaningFrequency"],
            TimeZoneInfo.Utc);
    }
}