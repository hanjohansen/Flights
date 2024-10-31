using Flights.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Flights.Infrastructure.Data;

public class FlightsDbContext : DbContext
{
    public virtual DbSet<GameEntity> Games { get; set; } = null!;
    public virtual DbSet<PlayerEntity> Players { get; set; } = null!;
    public virtual DbSet<PlayerFileEntity> PlayerFiles { get; set; } = null!;

    public FlightsDbContext(DbContextOptions<FlightsDbContext> options) 
        : base(options){}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<PlayerEntity>(entity => {
            entity.ToTable("players");
            entity.HasKey(x => x.Id);
            entity.HasMany(x => x.Games)
                .WithOne(x => x.Player);
        });

        builder.Entity<GameEntity>(entity => {
            entity.ToTable("games");
            entity.HasKey(x => x.Id);
            entity.HasMany(x => x.Players)
                .WithOne(x => x.Game);
        });

        builder.Entity<GamePlayerEntity>(entity => {
            entity.ToTable("game_players");
            entity.HasKey(x => x.Id);
            entity.HasOne(x => x.Game)
                .WithMany(x => x.Players)
                .HasForeignKey(x => x.GameId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x => x.Player)
                .WithMany(x => x.Games)
                .HasForeignKey(x => x.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<GameRoundEntity>(entity => {
            entity.ToTable("game_rounds");
            entity.HasKey(x => x.Id);
            entity.HasOne(x => x.Game)
                .WithMany(x => x.Rounds)
                .HasForeignKey(x => x.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<RoundStatEntity>(entity => {
            entity.ToTable("round_stat");
            entity.HasKey(x => x.Id);
            entity.HasOne(x => x.Round)
                .WithMany(x => x.RoundStats)
                .HasForeignKey(x => x.RoundId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.OwnsOne(x => x.FirstDart);
            entity.OwnsOne(x => x.SecondDart);
            entity.OwnsOne(x => x.ThirdDart);

            entity.HasOne(x => x.Player)
                .WithMany()
                .HasForeignKey(x => x.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<PlayerFileEntity>(entity => {
            entity.ToTable("player_files");
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.Player)
                .WithMany(x => x.Files)
                .HasForeignKey(x => x.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }    

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder
            .Properties<Enum>()
            .HaveConversion<string>();
    }
}