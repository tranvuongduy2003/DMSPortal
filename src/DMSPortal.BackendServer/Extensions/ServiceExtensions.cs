using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DMSPortal.BackendServer.Data;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Infrastructure.Interfaces;
using DMSPortal.BackendServer.Infrastructure.RepositoryBase;
using DMSPortal.BackendServer.Repositories;
using DMSPortal.BackendServer.Repositories.Contracts;
using DMSPortal.BackendServer.Services;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.Configurations;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.Console.Extensions;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DMSPortal.BackendServer.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration,
        string appCors)
    {
        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
                options.JsonSerializerOptions.DefaultIgnoreCondition =
                    JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

        services.AddCors(p =>
            p.AddPolicy(appCors, build =>
            {
                build
                    .WithOrigins("*")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();
        services
            .AddAutoMapper(config => config.AddProfile(new MappingProfile()));

        services.ConfigureSwagger();
        services.ConfigureAppSettings(configuration);
        services.ConfigureApplication();
        services.ConfigureApplicationDbContext(configuration);
        services.ConfigureRedis(configuration);
        services.ConfigureHangfireServices();
        services.ConfigureAuthetication();
        services.ConfigureInfrastructureServices();

        return services;
    }

    public static IServiceCollection ConfigureAppSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(nameof(JwtOptions))
            .Get<JwtOptions>();
        services.AddSingleton<JwtOptions>(jwtOptions);

        var emailSettings = configuration.GetSection(nameof(EmailSettings))
            .Get<EmailSettings>();
        services.AddSingleton<EmailSettings>(emailSettings);

        var hangfireSettings = configuration.GetSection(nameof(HangfireSettings))
            .Get<HangfireSettings>();
        services.AddSingleton<HangfireSettings>(hangfireSettings);

        return services;
    }

    private static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(option =>
        {
            option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme,
                securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization PageOrder as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    private static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        services.Configure<IdentityOptions>(options =>
        {
            // Default Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.User.RequireUniqueEmail = true;
        });
        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromHours(8);
        });
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
        });
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

        return services;
    }

    private static IServiceCollection ConfigureAuthetication(this IServiceCollection services)
    {
        var jwtOptions = services.GetOptions<JwtOptions>("JwtOptions");
        var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);


        services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                x.SaveToken = true;
            });

        return services;
    }

    private static IServiceCollection ConfigureApplicationDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnectionString");
        if (connectionString == null || string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException("DefaultConnectionString is not configured.");
        services.AddDbContext<ApplicationDbContext>(m => m.UseNpgsql(connectionString));
        return services;
    }

    public static IServiceCollection ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CacheConnectionString");
        if (connectionString == null || string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException("CacheConnectionString is not configured.");

        // Redis Configuration
        services.AddStackExchangeRedisCache(options => { options.Configuration = connectionString; });

        return services;
    }

    public static IServiceCollection ConfigureHangfireServices(this IServiceCollection services)
    {
        var hangfireSettings = services.GetOptions<HangfireSettings>("HangfireSettings");
        if (hangfireSettings == null
            || hangfireSettings.Storage == null
            || string.IsNullOrEmpty(hangfireSettings.Storage.ConnectionString)
            || string.IsNullOrEmpty(hangfireSettings.Storage.DBProvider))
            throw new ArgumentNullException("HangfireSettings is not configured properly!");

        var mongoUrlBuilder = new MongoUrlBuilder(hangfireSettings.Storage.ConnectionString);

        var mongoClientSettings = MongoClientSettings.FromUrl(
            new MongoUrl(hangfireSettings.Storage.ConnectionString));
        mongoClientSettings.SslSettings = new SslSettings()
        {
            EnabledSslProtocols = SslProtocols.Tls12
        };
        var mongoClient = new MongoClient(mongoClientSettings);
        var mongoStorageOptions = new MongoStorageOptions()
        {
            MigrationOptions = new MongoMigrationOptions()
            {
                MigrationStrategy = new MigrateMongoMigrationStrategy(),
                BackupStrategy = new CollectionMongoBackupStrategy()
            },
            CheckConnection = true,
            Prefix = "SchedulerQueue",
            CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
        };

        services.AddHangfire((provider, config) =>
        {
            config.UseSimpleAssemblyNameTypeSerializer()
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMongoStorage(mongoClient, mongoUrlBuilder.DatabaseName, mongoStorageOptions);

            var jsonSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
            };
            config.UseSerializerSettings(jsonSettings);
        });
        services.AddHangfireConsoleExtensions();
        services.AddHangfireServer(options => { options.ServerName = hangfireSettings.ServerName; });

        return services;
    }

    private static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services) =>
        services.AddTransient<DbInitializer>()
            .AddTransient<IEmailService, EmailService>()
            .AddTransient<ITokenService, TokenService>()
            .AddTransient<ICacheService, CacheService>()
            .AddTransient<IHangfireService, HangfireService>()
            .AddTransient<ISerializeService, SerializeService>()
            .AddScoped<IAttendancesService, AttendancesService>()
            .AddTransient<IAuthService, AuthService>()
            .AddTransient<IBranchesService, BranchesService>()
            .AddTransient<IClassesService, ClassesService>()
            .AddTransient<INotesService, NotesService>()
            .AddTransient<IPitchesService, PitchesService>()
            .AddTransient<IPitchGroupsService, PitchGroupsService>()
            .AddTransient<IShiftsService, ShiftsService>()
            .AddTransient<IStudentsService, StudentsService>()
            .AddTransient<ICommandsService, CommandsService>()
            .AddTransient<IPermissionsService, PermissionsService>()
            .AddTransient<IFunctionsService, FunctionsService>()
            .AddScoped(typeof(IRepositoryQueryBase<,>), typeof(RepositoryQueryBase<,>))
            .AddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>))
            .AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork))
            .AddScoped<IAttendancesRepository, AttendancesRepository>()
            .AddScoped<IBranchesRepository, BranchesRepository>()
            .AddScoped<IClassesRepository, ClassesRepository>()
            .AddScoped<IClassInShiftsRepository, ClassInShiftsRepository>()
            .AddScoped<ICommandInFunctionsRepository, CommandInFunctionsRepository>()
            .AddScoped<ICommandsRepository, CommandsRepository>()
            .AddScoped<IFunctionsRepository, FunctionsRepository>()
            .AddScoped<INotesRepository, NotesRepository>()
            .AddScoped<IPermissionsRepository, PermissionsRepository>()
            .AddScoped<IPitchesRepository, PitchesRepository>()
            .AddScoped<IPitchGroupsRepository, PitchGroupsRepository>()
            .AddScoped<IShiftsRepository, ShiftsRepository>()
            .AddScoped<IStudentInClassesRepository, StudentInClassesRepository>()
            .AddScoped<IStudentsRepository, StudentsRepository>();
}