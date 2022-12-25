using MediatR;
using Microsoft.EntityFrameworkCore;
using Repositories.Realizations;
using Streetcode.BLL.Interfaces.AdditionalContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Interfaces.Media;
using Streetcode.BLL.Interfaces.Media.Images;
using Streetcode.BLL.Interfaces.Partners;
using Streetcode.BLL.Interfaces.Streetcode;
using Streetcode.BLL.Interfaces.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Timeline;
using Streetcode.BLL.Interfaces.Toponyms;
using Streetcode.BLL.Interfaces.Transactions;
using Streetcode.BLL.Services.AdditionalContent;
using Streetcode.BLL.Services.Logging;
using Streetcode.BLL.Services.Media;
using Streetcode.BLL.Services.Media.Images;
using Streetcode.BLL.Services.Partners;
using Streetcode.BLL.Services.Streetcode;
using Streetcode.BLL.Services.Streetcode.TextContent;
using Streetcode.BLL.Services.Timeline;
using Streetcode.BLL.Services.Toponyms;
using Streetcode.BLL.Services.Transactions;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Base;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(ILoggerService<>), typeof(LoggerService<>));

        services.AddScoped<IFactService, FactService>();
        services.AddScoped<IAudioService, AudioService>();
        services.AddScoped<IArtService, ArtService>();
        services.AddScoped<IVideoService, VideoService>();
        services.AddScoped<IImageService, ImageService>();
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
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddAutoMapper(typeof(Program).Assembly);
        services.AddMediatR(typeof(Program).Assembly);

        services.AddCors(opt =>
        {
            opt.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.WithOrigins("http://localhost:3000");
                policy.AllowCredentials();
                policy.SetPreflightMaxAge(TimeSpan.FromDays(1));
            });
        });

        services.AddHsts(opt =>
        {
            opt.Preload = true;
            opt.IncludeSubDomains = true;
            opt.MaxAge = TimeSpan.FromDays(30);
        });

        services.AddLogging();
    }
}