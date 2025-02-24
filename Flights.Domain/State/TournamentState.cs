using Flights.Domain.Entities.Game;
using Flights.Domain.Entities.Tournament;

namespace Flights.Domain.State;

public record TournamentState(
    Guid Id,
    GameType Type,
    int X01Target,
    InOutModifier InModifier,
    InOutModifier OutModifier,
    List<TournamentPlayerState> Players,
    List<TournamentRoundState> Rounds,
    bool Finished)
{
    public static TournamentState FromEntity(TournamentEntity entity)
    {
        return new TournamentState(
            Id: entity.Id,
            Type: entity.Type,
            X01Target: entity.X01Target,
            InModifier: entity.InModifier,
            OutModifier: entity.OutModifier,
            Players: entity.Players.Select(x => TournamentPlayerState.FromEntity(x)).ToList(),
            Rounds: entity.Rounds.Select(TournamentRoundState.FromEntity).ToList(),
            Finished: entity.Finished != null);
    }
}

public record TournamentPlayerState(
    Guid Id,
    string Name,
    int? Rank = null)
{
    public static TournamentPlayerState FromEntity(TournamentPlayerEntity entity, int? rank)
    {
        return new TournamentPlayerState(
            entity.PlayerId,
            entity.Player.Name,
            rank);
    }
    
    public static TournamentPlayerState FromEntity(TournamentPlayerEntity entity)
    {
        return new TournamentPlayerState(
            entity.PlayerId,
            entity.Player.Name,
            entity.Rank);
    }
}

public record TournamentRoundState(
    int Number,
    TournamentPlayerState? Wildcard,
    List<TournamentGameState> Games
)
{
    public static TournamentRoundState FromEntity(TournamentRoundEntity entity)
    {
        var wildcard = entity.WildCard != null
            ? new TournamentPlayerState(entity.WildCard.PlayerId, entity.WildCard.Player.Name)
            : null;
        
        return new TournamentRoundState(
            Number: entity.OrderNumber,
            Wildcard: wildcard,
            Games: entity.Games.Select(TournamentGameState.FromEntity).ToList());
    }
}

public record TournamentGameState(
    Guid? Id,
    int Number,
    bool Finished,
    bool Started,
    bool IsSemiFinal,
    bool IsLosersCup,
    bool IsLosersCupReady,
    List<TournamentPlayerState> Players)
{
    public static TournamentGameState FromEntity(TournamentGameEntity entity)
    {
        var players = new List<TournamentPlayerState>();

        var semiFinalRound = entity.TournamentRound.Games.Count(x => x.IsLosersCup == false) == 2 && entity.TournamentRound.WildCard == null;
        
        if(entity.Game != null)
            foreach (var player in entity.Game.Rounds.Last().RoundStats)
            {
                var state = new TournamentPlayerState(player.PlayerId, player.Player.Name, player.Rank);
                players.Add(state);
            }

        if (entity.IsLosersCup && entity.Game == null)
        {
            var otherGamesLosers = entity.TournamentRound.Games
                .Where(x => !x.IsLosersCup && x.Game!.Finished != null)
                .SelectMany(y => y.Game!.Rounds.Last().RoundStats
                    .Where(z => z.Rank != 1)
                    .Select(q => q.Player))
                .ToList();

            foreach (var loser in otherGamesLosers)
                players.Add(new TournamentPlayerState(loser.Id, loser.Name));
        }

        var started = entity.Game?.Rounds
            .Any(y => y.RoundStats
                .Any(z => z.FirstDart != null || z.SecondDart != null || z.ThirdDart != null)); 
        
        return new TournamentGameState(
            Id: entity.Game?.Id,
            Number: entity.OrderNumber,
            Finished: entity.Game?.Finished != null,
            Started: started ?? false,
            IsSemiFinal: semiFinalRound,
            IsLosersCup: entity.IsLosersCup,
            IsLosersCupReady: entity.IsLosersCup && entity.Game != null,
            Players: players);
    }
}