using EFTask.Persistence;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Interfaces;
using Repositories.Realizations;
using Services.Interfaces;
using Services.Services;
using StreetCode.DAL.Repositories.Interfaces.Base;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IFactService, FactService>();
        services.AddScoped<IMediaService, MediaService>();
        services.AddScoped<IPartnersService, PartnersService>();
        services.AddScoped<IStreetcodeService, StreetcodeService>();
        services.AddScoped<ISubtitleService, SubtitleService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<ITermService, TermService>();
        services.AddScoped<ITextService, TextService>();
        services.AddScoped<ITimelineService, TimelineService>();
        services.AddScoped<IToponymService, ToponymService>();
        services.AddScoped<ITransactLinksService, TransactLinksService>();

        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

        return services;
    }

    public static void AddCustomServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<StreetcodeDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DmytroBConnection"))
        );
    }
}

