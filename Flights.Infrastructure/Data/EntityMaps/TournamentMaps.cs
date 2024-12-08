using Flights.Domain.Entities.Game;
using Flights.Domain.Entities.Tournament;
using Microsoft.EntityFrameworkCore;

namespace Flights.Infrastructure.Data.EntityMaps;

public static  class TournamentMaps
{
    public static void Configure(ModelBuilder builder)
    {
        builder.Entity<TournamentEntity>(entity =>
        {
            entity.ToTable("tournaments");
            entity.HasKey(x => x.Id);
            entity.HasMany(x => x.Players)
                .WithOne(x => x.Tournament)
                .HasForeignKey(x => x.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(x => x.Rounds)
                .WithOne(x => x.Tournament)
                .HasForeignKey(x => x.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<TournamentPlayerEntity>(entity =>
        {
            entity.ToTable("tournament_players");
            entity.HasKey(x => x.Id);
            entity.HasOne(x => x.Player)
                .WithMany(x => x.Tournaments)
                .HasForeignKey(x => x.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<TournamentRoundEntity>(entity =>
        {
            entity.ToTable("tournament_rounds");
            entity.HasKey(x => x.Id);
            entity.HasOne(x => x.WildCard)
                .WithMany()
                .HasForeignKey(x => x.WildCardId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<TournamentGameEntity>(entity =>
        {
            entity.ToTable("tournament_games");
            entity.HasKey(x => x.Id);
            entity.HasOne(x => x.Game)
                .WithOne(x => x.TournamentGame)
                .HasForeignKey<GameEntity>(x => x.TournamentGameId);
        });

    }
}