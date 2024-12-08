using Flights.Domain.Entities;
using Flights.Domain.Entities.Game;
using Microsoft.EntityFrameworkCore;
namespace Flights.Infrastructure.Data.EntityMaps;

public static class GameMap
{
    public static void Configure(ModelBuilder builder)
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
}