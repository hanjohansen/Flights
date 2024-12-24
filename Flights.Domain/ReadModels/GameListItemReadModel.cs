using Flights.Domain.Entities.Game;
using Flights.Domain.Entities.Tournament;

namespace Flights.Domain.ReadModels;

public record GameListItemReadModel(
    Guid Id,
    bool IsTournament,
    GameType Type,
    int X01Target,
    DateTimeOffset Started,
    DateTimeOffset? Finished,
    List<GameItemPlayerReadModel> Players)
{
    public static GameListItemReadModel FromGame(GameEntity entity)
    {
        return new GameListItemReadModel(
            Id: entity.Id,
            IsTournament: false,
            Type: entity.Type,
            X01Target: entity.X01Target,
            Started: entity.Started,
            Finished: entity.Finished,
            Players: entity.Players.Select(x => x.Player.Name)
                .Select(y => new GameItemPlayerReadModel(y)).ToList());
    }
    
    public static GameListItemReadModel FromTournament(TournamentEntity entity)
    {
        return new GameListItemReadModel(
            Id: entity.Id,
            IsTournament: true,
            Type: entity.Type,
            X01Target: entity.X01Target,
            Started: entity.Started,
            Finished: entity.Finished,
            Players: entity.Players.Select(x => x.Player.Name)
                .Select(y => new GameItemPlayerReadModel(y)).ToList());
    } 
}

public record GameItemPlayerReadModel(string Name);