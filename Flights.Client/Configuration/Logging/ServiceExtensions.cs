using System.Reflection;
using Serilog;
using Serilog.Events;

namespace Flights.Client.Configuration.Logging;

public static class ServiceExtensions
{
    public static WebApplicationBuilder LogConfiguration(this WebApplicationBuilder builder)
    {
        var storageConfig = builder.Configuration.GetSection(StorageConfiguration.ConfigurationKey).Get<StorageConfiguration>();
        var authConfig = builder.Configuration.GetSection(AuthConfiguration.ConfigurationKey).Get<AuthConfiguration>();
        var appConfig = builder.Configuration.GetSection(AppConfiguration.ConfigurationKey).Get<AppConfiguration>();
        
        var version = "v" + Assembly.GetExecutingAssembly().GetName().Version; 
        
        Log.Information("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
        Log.Information("______ _ _       _     _       ");
        Log.Information("|  ___| (_)     | |   | |      ");
        Log.Information("| |_  | |_  __ _| |__ | |_ ___ ");
        Log.Information("|  _| | | |/ _` | '_ \\| __/ __|");
        Log.Information("| |   | | | (_| | | | | |_\\__ \\");
        Log.Information("\\_|   |_|_|\\__, |_| |_|\\__|___/   {0}", version);
        Log.Information("            __/ |              ");
        Log.Information("           |___/               ");
        Log.Information("----------------------------------------------------------------------------------------------------");
        
        Log.Information("Configuration");
        Log.Information("Environment : {0}", builder.Environment.EnvironmentName);
        Log.Information("DbProvider  : {0}", storageConfig?.DbProvider ?? "unknown");
        Log.Information("AutoLogin   : {0}", authConfig?.AllowAutoLogin.ToString() ?? "unknown");
        Log.Information("Logging     : {0}", appConfig?.UseLogging.ToString() ?? "unknown");
        Log.Information("RequestLog  : {0}", appConfig?.UseRequestLogging.ToString() ?? "unknown");
        Log.Information("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

        return builder;
    }
    
    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped(typeof(AppLogger<>));

        Log.Logger = builder.CreateLoggerConfiguration().CreateBootstrapLogger();
        
        builder.Host.UseSerilog((_, _, cfg) =>
        {
            cfg.CreateLoggerConfiguration(builder);
        });

        return builder;
    }

    private static LoggerConfiguration CreateLoggerConfiguration(this WebApplicationBuilder builder)
    {
        var cfg = new LoggerConfiguration();

        return cfg.CreateLoggerConfiguration(builder);
    }

    private static LoggerConfiguration CreateLoggerConfiguration(this LoggerConfiguration cfg, WebApplicationBuilder builder)
    {
        var appConfig = builder.Configuration.GetSection(AppConfiguration.ConfigurationKey).Get<AppConfiguration>();
        
        if(appConfig == null)
            throw new ApplicationException("App configuration is missing");

        if (appConfig.UseLogging == false)
            return cfg;
        
        if (builder.Environment.IsDevelopment())
        {
            cfg.MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [TID:{TenantId:u16}] [GID:{GameId}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    "logs/flights_logs_dev.txt", 
                    rollingInterval: RollingInterval.Day,
                    retainedFileTimeLimit: TimeSpan.FromDays(7),
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    outputTemplate:
                    "[{Timestamp:dd/MM/yy-HH:mm:ss} {Level:u3}] [TID:{TenantId:u16}] [GID:{GameId}] [{SourceContext}] {Message:lj}{NewLine}{Exception}");
        }
        else
        {
            cfg.MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [TID:{TenantId:u16}] [GID:{GameId}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    "logs/flights_logs_.txt", 
                    rollingInterval: RollingInterval.Month,
                    retainedFileTimeLimit: TimeSpan.FromDays(90),
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    outputTemplate:
                    "[{Timestamp:dd/MM/yy-HH:mm:ss} {Level:u3}] [TID:{TenantId:u16}] [GID:{GameId}] [{SourceContext}] {Message:lj}{NewLine}{Exception}");
        }

        return cfg;
    }

    public static WebApplication UseRequestLogging(this WebApplication app)
    {
        var appConfig = app.Configuration.Get<AppConfiguration>();
        
        if(appConfig == null)
            throw new ApplicationException("App configuration is missing");
        
        if (appConfig is { UseLogging: true, UseRequestLogging: true })
            app.UseSerilogRequestLogging();
            
        return app;
    }
}