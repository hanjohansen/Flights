using Flights.Domain.Entities.Tournament;
using Flights.Util;

namespace Flights.Domain.State.Solvers.Tournament;

public partial class TournamentSolver(TournamentEntity entity) : ITournamentSolver
{
    public TournamentState Solve()
    {
        CalculateTournamentState();
        return TournamentState.FromEntity(entity);
    }

    private void CalculateTournamentState()
    {
        if (!entity.Rounds.Any())
        {
            InitTournament();
            return;
        }
        
        //finished tournament?
        if (entity.Finished != null)
            return;

        var lastRound = entity.Rounds.Last();
        //are we finished?
        if (lastRound.Games.Count == 1 && 
            lastRound.Games.Single().Game!.Finished != null)
        {
            SetFirstAndSecondRank();
            lastRound.Games.Single().Game!.FinishLocked = true;
            entity.Finished = lastRound.Games.Single().Game!.Finished;
            return;
        }

        //if losers cup exists..
        if (lastRound.Games.Any(x => x.Game == null))
        {
            var allOtherFinished = lastRound.Games
                .Where(x => x.Game != null)
                .Select(x => x.Game)
                .All(x => x!.Finished != null);

            if (allOtherFinished)
            {
                InitLosersCup();
                return;
            }
        }
        
        SetLosers();

        var roundGames = lastRound.Games.Select(x => x.Game).ToList();
        var allGamesFinished = roundGames.All(x => x?.Finished != null);
        
        if (allGamesFinished)
        {
            var isSemiFinal = entity.SemiFinalWithLosersCup 
                && roundGames.Count == 3 
                && lastRound.WildCard == null;
                
            if(isSemiFinal)
                SetThirdRank();
            
            var winners = new List<TournamentPlayerEntity>();
            foreach (var game in lastRound.Games)
            {
                if (isSemiFinal && game.IsLosersCup)
                    continue;
                
                var lastGameRound = game.Game!.Rounds.Last();
                var winner = lastGameRound.RoundStats.First(x => x.Rank == 1);
                var tournamentPlayer = entity.Players.First(x => x.PlayerId == winner.Player.Id);
                winners.Add(tournamentPlayer);
            }
            
            winners.Shuffle();
            
            if(lastRound.WildCard != null)
                winners.Insert(0, lastRound.WildCard);
            
            InitNewRound(winners);
        }
    }
}