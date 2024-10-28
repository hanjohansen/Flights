using Microsoft.EntityFrameworkCore;
using Flights.Infrastructure.Data;
using Flights.Infrastructure.Data.Repos;
using Flights.Infrastructure.Port;
using Flights.Client.Service.Port.FileStorage;
using Flights.Client.Service.FileStorage;
using Flights.Client.Service.Port;
using Flights.Client.Service;

namespace Flights.Client;

public static class ServiceExtensions
{
    public static WebApplicationBuilder AddGamesDatabase(this WebApplicationBuilder builder){

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContextFactory<FlightsDbContext>(options => {
            options.UseSqlite(connectionString);
        });

        builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
        builder.Services.AddScoped<IPlayerFileRepository, PlayerFileRepository>();
        builder.Services.AddScoped<IGameRepository, GameRepository>();
        builder.Services.AddScoped<IStatRepository, StatRepository>();
        
        return builder;
    }

    public static WebApplicationBuilder AddFileStorage(this WebApplicationBuilder builder){

        builder.Services.AddScoped<IFileStorage, BaseFileStorage>();
        builder.Services.AddScoped<IJingleFileStorage, JingleFileStorage>();
        builder.Services.AddScoped<IJingleFileUploadService, JingleFileUploadService>();
        
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