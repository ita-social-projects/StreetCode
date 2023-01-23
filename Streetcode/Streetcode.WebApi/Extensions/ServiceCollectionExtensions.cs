using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddEntityServices(this IServiceCollection services)
    {
        services.AddScoped<IFactService, FactService>();
        services.AddScoped<IAudioService, AudioService>();
        services.AddScoped<IArtService, ArtService>();
        services.AddScoped<IVideoService, VideoService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IPartnersService, PartnersService>();
        services.AddScoped<IRelatedFigureService, RelatedFigureService>();
        services.AddScoped<IStreetcodeService, StreetcodeService>();
        services.AddScoped<ISubtitleService, SubtitleService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<ITermService, TermService>();
        services.AddScoped<ITextService, TextService>();
        services.AddScoped<ITimelineItemService, TimelineItemService>();
        services.AddScoped<IToponymService, ToponymService>();
        services.AddScoped<ITransactLinksService, TransactLinksService>();
    }

    public static void AddRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
    }

    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddEntityServices();
        services.AddRepositoryServices();

        var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.AddAutoMapper(currentAssemblies);
        services.AddMediatR(currentAssemblies);

        services.AddScoped(typeof(ILoggerService<>), typeof(LoggerService<>));
    }

    public static void AddApplicationServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<StreetcodeDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), opt =>
            {
                opt.MigrationsAssembly(typeof(StreetcodeDbContext).Assembly.GetName().Name);
                opt.MigrationsHistoryTable("__EFMigrationsHistory", schema: "entity_framework");
            });
        });

        services.AddCors(opt =>
        {
            opt.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin();
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
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
        services.AddControllers();
    }

    public static void AddSwaggerServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyApi", Version = "v1" });

            opt.CustomSchemaIds(x => x.FullName);
        });
    }
}