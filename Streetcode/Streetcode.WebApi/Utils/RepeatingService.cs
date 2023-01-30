using Streetcode.DAL.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Streetcode.WebApi.Utils;

public class RepeatingService : BackgroundService
{
    // change FromDays function if you want to change periodicity of parsing data from ukrposhta
    private readonly PeriodicTimer _timer = new(TimeSpan.FromDays(20));
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RepeatingService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        do
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();

            StreetcodeDbContext streetcodeDbContext =
                scope.ServiceProvider.GetRequiredService<StreetcodeDbContext>();

            WebParsingUtils webParsingUtils =
                new WebParsingUtils(streetcodeDbContext);

            await webParsingUtils.ParseZipFileFromWebAsync();
        }
        while (await _timer.WaitForNextTickAsync(stoppingToken)
            && !stoppingToken.IsCancellationRequested);
    }
}