
namespace Flights.Domain.State.Solvers.Tournament;

public partial class TournamentSolver
{
    private void SetThirdRank()
    {
        if(!entity.SemiFinalWithLosersCup)
            return;

        var lastRound = entity.Rounds.Last();
        var game = lastRound.Games.First(x => x.IsLosersCup).Game;
        var winner = game!.Rounds.Last().RoundStats.First(x => x.Rank == 1);
        
        var tPlayer = entity.Players.First(x => x.PlayerId == winner.Player.Id);
        tPlayer.Rank = 3;
    }

    private void SetFirstAndSecondRank()
    {
        var lastGame = entity.Rounds.Last().Games.Single().Game!;

        foreach (var player in lastGame.Rounds.Last().RoundStats)
        {
            var tPlayer = entity.Players.First(x => x.PlayerId == player.Player.Id);
            tPlayer.Rank = player.Rank;
        }
    }
}