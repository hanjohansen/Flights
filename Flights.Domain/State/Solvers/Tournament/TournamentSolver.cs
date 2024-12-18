using Flights.Domain.Entities.Tournament;

namespace Flights.Domain.State.Solvers.Tournament;

public partial class TournamentSolver(TournamentEntity entity) : ITournamentSolver
{
    public TournamentState Solve()
    {
        CalculateTournamentState();
        return Projection();
    }

    private TournamentState Projection()
    {
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
            entity.Finished = lastRound.Games.Single().Game!.Finished;
            return;
        }

        //if losers cup axists..
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

        var allGamesFinished = lastRound.Games.Select(x => x.Game).All(x => x?.Finished != null);
        if (allGamesFinished)
        {
            var winners = new List<TournamentPlayerEntity>();
            foreach (var game in lastRound.Games)
            {
                var lastGameRound = game.Game!.Rounds.Last();
                var winner = lastGameRound.RoundStats.First(x => x.Rank == 1);
                var tournamentPlayer = entity.Players.First(x => x.PlayerId == winner.Player.Id);
                winners.Add(tournamentPlayer);
            }
            
            if(lastRound.WildCard != null)
                winners.Insert(0, lastRound.WildCard);
            
            InitNewRound(winners);
        }
    }
}