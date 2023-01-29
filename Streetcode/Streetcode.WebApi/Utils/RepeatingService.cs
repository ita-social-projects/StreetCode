using Streetcode.DAL.Persistence;

namespace Streetcode.WebApi.Utils;

public class RepeatingService : BackgroundService
{
    // change FromDays function if you want to change periodicity of parsing data from ukrposhta
    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(20));
    private readonly WebParsingUtils _utils;

    public RepeatingService(StreetcodeDbContext streetcodeDbContext)
    {
        _utils = new WebParsingUtils(streetcodeDbContext);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        do
        {
            await _utils.ParseZipFileFromWebAsync();
        }
        while (await _timer.WaitForNextTickAsync(stoppingToken)
            && !stoppingToken.IsCancellationRequested);
    }
}