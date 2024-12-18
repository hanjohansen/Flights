using Flights.Domain.Entities.Tournament;

namespace Flights.Domain.State;

public record TournamentState(
    Guid TournamentId,
    List<TournamentPlayerState> Players)
{
    public static TournamentState FromEntity(TournamentEntity entity)
    {
        return new TournamentState(
            TournamentId: entity.Id,
            Players: entity.Players.Select(TournamentPlayerState.FromEntity).ToList());
    }
};

public record TournamentPlayerState(string Name)
{
    public static TournamentPlayerState FromEntity(TournamentPlayerEntity entity)
    {
        return new TournamentPlayerState(entity.Player.Name);
    }
}

public record TournamentRoundState();