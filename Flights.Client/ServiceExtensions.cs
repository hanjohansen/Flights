using Flights.Client.Configuration;
using Flights.Client.Authentication;
using Microsoft.EntityFrameworkCore;
using Flights.Infrastructure.Data;
using Flights.Infrastructure.Data.Repos;
using Flights.Infrastructure.Port;
using Flights.Client.Service.Port.FileStorage;
using Flights.Client.Service.FileStorage;
using Flights.Client.Service.Port;
using Flights.Client.Service;
using Flights.Storage.MySql;
using Flights.Storage.Sqlite;
using Flights.Infrastructure.Security;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using MudBlazor.Services;

namespace Flights.Client;

public static class ServiceExtensions
{
    public static WebApplicationBuilder AddUiServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IThemeInfoProvider, ThemeInfoProvider>();
        builder.Services.AddScoped<IBrowserStorage, BrowserStorage>();
        
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddMudServices();

        return builder;
    }

    public static WebApplicationBuilder AddAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddMemoryCache();
        builder.Services.AddAuthorization();
        
        builder.Services.AddScoped<ITenantService, TenantService>();
        builder.Services.AddScoped<IHashingService, BCryptor>();
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<IUserSessionCache, UserSessionCache>();
        builder.Services.AddScoped<AuthenticationStateProvider, FlightsAuthenticationStateProvider>();
        
        return builder;
    }
    
    public static WebApplicationBuilder AddGamesDatabase(this WebApplicationBuilder builder){
        
        var storageConfig = builder.Configuration.GetSection("Storage").Get<StorageConfiguration>();
        
        if(storageConfig == null)
            throw new ApplicationException("Storage configuration is missing");

        switch (storageConfig.DbProvider)
        {
            case "Sqlite":
                builder.Services.AddDbContextFactory<FlightsDbContext>(options => {
                    options.UseSqlite(storageConfig.ConnectionString, opts => { opts.MigrationsAssembly(typeof(StorageSqlite).Assembly.GetName().Name!); });
                });
                break;
            case "MySql":
                builder.Services.AddDbContextFactory<FlightsDbContext>(options =>
                {
                    options.UseMySql(storageConfig.ConnectionString,ServerVersion.AutoDetect(storageConfig.ConnectionString), opts => { opts.MigrationsAssembly(typeof(StorageMySql).Assembly.GetName().Name!); });
                });
                break;
            default:
                throw new ApplicationException($"Unknown DbProvider '{storageConfig.DbProvider}'");
        }

        builder.Services.AddScoped<ITenantRepository, TenantRepository>();
        builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
        builder.Services.AddScoped<IPlayerFileRepository, PlayerFileRepository>();
        builder.Services.AddScoped<IGameRepository, GameRepository>();
        builder.Services.AddScoped<IStatRepository, StatRepository>();
        builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
        
        return builder;
    }

    public static WebApplicationBuilder AddFileStorage(this WebApplicationBuilder builder){

        builder.Services.AddScoped<IFileStorage, BaseFileStorage>();
        builder.Services.AddScoped<IJingleFileStorage, JingleFileStorage>();
        builder.Services.AddScoped<IJingleFileUploadService, JingleFileUploadService>();
        
        return builder;
    }
    
    public static WebApplicationBuilder AddSignalRServices(this WebApplicationBuilder builder){

        builder.Services.AddSignalR();
        builder.Services.AddResponseCompression(opts =>
        {
            opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                ["application/octet-stream"]);
        });
        
        return builder;
    }

    public static WebApplication UseDatabaseMigrations(this WebApplication app){

        using (var scope =  app.Services.CreateScope())
        using (var context = scope.ServiceProvider.GetService<FlightsDbContext>())
            if (context == null)
                throw new InvalidDataException("Unable to get db context for migrations");
            else
                context.Database.Migrate();

        return app;  
    }
}