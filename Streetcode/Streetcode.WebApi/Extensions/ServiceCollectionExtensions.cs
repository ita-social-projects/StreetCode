using System.Text;
using System.Net.Security;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.Base;
using Streetcode.BLL.Interfaces.Email;
using Streetcode.BLL.Services.Email;
using Streetcode.DAL.Entities.AdditionalContent.Email;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Services.BlobStorageService;
using Streetcode.BLL.Services.AudioService;
using Streetcode.BLL.Interfaces.Authentication;
using Streetcode.BLL.Services.Authentication;
using Microsoft.FeatureManagement;
using Streetcode.BLL.Interfaces.Audio;
using Streetcode.BLL.Interfaces.Payment;
using Streetcode.BLL.Services.Payment;
using Streetcode.BLL.Interfaces.Instagram;
using Streetcode.BLL.Services.Instagram;
using Streetcode.BLL.Interfaces.Text;
using Streetcode.BLL.Services.Text;
using Streetcode.BLL.Services.CacheService;
using Streetcode.BLL.Interfaces.Cache;
using Streetcode.BLL.Interfaces.Image;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.ImageService;
using Streetcode.BLL.Services.Logging;
using Streetcode.WebApi.Utils;
using Microsoft.AspNetCore.Identity;
using Streetcode.BLL.PipelineBehaviour;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
    }

    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddRepositoryServices();
        services.AddFeatureManagement();
        services.AddMemoryCache();
        services.AddHttpContextAccessor();

        var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.AddAutoMapper(currentAssemblies);
        services.AddMediatR(currentAssemblies);
        services.AddScoped<ILoggerService, LoggerService>();
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        services.AddSingleton<ICacheService, CacheService>();
        services.AddScoped<IBlobService, BlobService>();
        services.AddScoped<IAudioService, AudioService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICaptchaService, CaptchaService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IInstagramService, InstagramService>();
        services.AddScoped<ITextService, AddTermsToTextService>();
    }

    public static void AddApplicationServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
        services.AddSingleton(emailConfig);

        services.AddDbContext<StreetcodeDbContext>(options =>
        {
            options.UseSqlServer(connectionString, opt =>
            {
                opt.MigrationsAssembly(typeof(StreetcodeDbContext).Assembly.GetName().Name);
                opt.MigrationsHistoryTable("__EFMigrationsHistory", schema: "entity_framework");
            });
        });

        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<StreetcodeDbContext>()
            .AddTokenProvider<DataProtectorTokenProvider<User>>(configuration["JWT:Issuer"]);

        services.AddHangfire(config =>
        {
            config.UseSqlServerStorage(connectionString);
        });

        services.AddHangfireServer();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

        var corsSettings = SettingsExtracter.GetCorsSettings(configuration);
        services.AddCors(opt =>
        {
            opt.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(corsSettings.AllowedOrigins)
                   .WithHeaders(corsSettings.AllowedHeaders)
                   .WithMethods(corsSettings.AllowedMethods)
                   .WithExposedHeaders(corsSettings.ExposedHeaders)
                   .SetPreflightMaxAge(TimeSpan.FromSeconds(corsSettings.PreflightMaxAge));
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
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}