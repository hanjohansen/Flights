using Flights.Domain.Entities;

namespace Flights.Domain.State.Solvers;

public class SolverBase
{
    public SolverBase(GameEntity entity){
        Game = entity;
    }

    public GameEntity Game {get;}

    public GameState FinalizeGame(){
        var lastRound = Game.Rounds.Last();

        var leftPlayers = lastRound.RoundStats.Where(x => x.Rank == null);
        var points = leftPlayers.Select(x => x.StartPoints)
            .Distinct()
            .OrderByDescending(x => x)
            .ToList();

        if(Game.Type == GameType.CtCricket)
            points.Reverse();

        var rank = lastRound.RoundStats.Max(x => x.Rank) ?? 1;

        foreach(var point in points){
            var pointPlayers = leftPlayers.Where(x => x.StartPoints == point).ToList();
            pointPlayers.ForEach(x => x.Rank = rank);
            rank++;
        }

        Game.Finished = DateTimeOffset.UtcNow;

        var solver = GameSolverFactory.GetGameSolver(Game);
        return solver.Solve();
    }
}