using Flights.Domain.Entities;
using Flights.Domain.Entities.Game;
using Flights.Infrastructure.Data.EntityMaps;
using Microsoft.EntityFrameworkCore;

namespace Flights.Infrastructure.Data;

public class FlightsDbContext(DbContextOptions<FlightsDbContext> options) : DbContext(options)
{
    public virtual DbSet<GameEntity> Games { get; set; } = null!;
    public virtual DbSet<PlayerEntity> Players { get; set; } = null!;
    public virtual DbSet<PlayerFileEntity> PlayerFiles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
       builder.ApplyAppModels();
    }    

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder
            .Properties<Enum>()
            .HaveConversion<string>();
    }
}