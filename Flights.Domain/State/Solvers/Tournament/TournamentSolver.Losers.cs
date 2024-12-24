using Flights.Domain.Entities.Game;

namespace Flights.Domain.State.Solvers.Tournament;

public partial class TournamentSolver
{
    private void SetLosers()
    {
        var lastRound = entity.Rounds.Last();
        var hasLosersCup = lastRound.Games.Any(x => x.IsLosersCup);
    
        var gameLosers = hasLosersCup
            ? SetLosersFromLosersCup()
            : SetLosersFromRound();
        
        foreach (var gameLoser in gameLosers)
        {
            var tPlayer = entity.Players.First(x => x.PlayerId == gameLoser.Player.Id);
            tPlayer.Rank = 0;
        }
    }
    
    private List<RoundStatEntity> SetLosersFromRound()
    {
        var lastRound = entity.Rounds.Last();
        var games = lastRound.Games
            .Where(x => x.Game != null && x.Game.Finished != null)
            .Select(y => y.Game!)
            .ToList();
    
        var gameLosers = games.SelectMany(x => x.Rounds.Last().RoundStats
                .Where(y => y.Rank != 1))
            .ToList();
    
        return gameLosers;
    }
    
    private List<RoundStatEntity> SetLosersFromLosersCup()
    {
        var gameLosers = new List<RoundStatEntity>();
        
        var lastRound = entity.Rounds.Last();
        var losersCup = lastRound.Games
            .First(x => x.IsLosersCup);

        if (losersCup.Game?.Finished == null)
            return gameLosers;
        
        gameLosers = losersCup.Game!.Rounds.Last().RoundStats
            .Where(y => y.Rank != 1)
            .ToList();
    
        return gameLosers;
    }
}