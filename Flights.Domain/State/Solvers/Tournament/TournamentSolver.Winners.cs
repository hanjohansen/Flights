using Flights.Domain.Entities.Game;
using Flights.Domain.Exception;

namespace Flights.Domain.State.Solvers.Tournament;

public partial class TournamentSolver
{
    private void SetThirdRank()
    {
        var thirdRanks = entity.SemiFinalWithLosersCup
            ? ThirdRankFromLosersCup()
            : ThirdRankFromLastTwoGame();
        
        foreach (var third in thirdRanks)
        {
            var tPlayer = entity.Players.First(x => x.PlayerId == third.Player.Id);
            tPlayer.Rank = 3;
        }
    }

    private List<RoundStatEntity> ThirdRankFromLosersCup()
    {
        var lastRound = entity.Rounds.Last();
        var game = lastRound.Games.First(x => x.IsLosersCup).Game;
        var winner = game!.Rounds.Last().RoundStats.First(x => x.Rank == 1);
        return [winner];
    }

    private List<RoundStatEntity> ThirdRankFromLastTwoGame()
    {
        var lastRound = entity.Rounds.Last();
        var firstGame = lastRound.Games.First().Game!;
        var secondGame = lastRound.Games.Last().Game!;
        var loserA = firstGame.Rounds.Last().RoundStats.First(x => x.Rank != 1);
        var loserB = secondGame.Rounds.Last().RoundStats.First(x => x.Rank != 1);
        
        //evaluate winner based on game
        //XO1
        if (entity.Type == GameType.X01)
        {
            //when points are equal..
            if (loserA.EndPoints == loserB.EndPoints)
            {
                //..and rounds taken are equal
                if (firstGame.Rounds.Count == secondGame.Rounds.Count)
                    return [loserA, loserB]; //both players are third

                var shorterGame = new List<GameEntity>() { firstGame, secondGame }.MinBy(x => x.Rounds.Count);
                var loserOfShorterGame = shorterGame!.Rounds.Last().RoundStats.First(x => x.Rank != 1);
                return [loserOfShorterGame];
            }

            var betterLoser = new List<RoundStatEntity>() { loserA, loserB }.MinBy(x => x.EndPoints);
            return [betterLoser!];
        }

        //crickets
        if (entity.Type == GameType.Cricket || entity.Type == GameType.CtCricket)
        {
            //when points are equal..
            if (loserA.EndPoints == loserB.EndPoints)
            {
                //..and rounds taken are equal
                if (firstGame.Rounds.Count == secondGame.Rounds.Count)
                    return [loserA, loserB]; //both players are third

                var shorterGame = new List<GameEntity>() { firstGame, secondGame }.MinBy(x => x.Rounds.Count);
                var loserOfShorterGame = shorterGame!.Rounds.Last().RoundStats.First(x => x.Rank != 1);
                return [loserOfShorterGame];
            }

            var losers = new List<RoundStatEntity>() { loserA, loserB };
            var betterLoser = entity.Type == GameType.Cricket
                ? losers.MaxBy(x => x.EndPoints)
                : losers.MinBy(x => x.EndPoints);
            
            return [betterLoser!];
        }
        
        //around the clock
        if (entity.Type == GameType.AroundTheClock)
        {
            //when points are equal..
            if (loserA.EndPoints == loserB.EndPoints)
            {
                return [loserA, loserB]; //both players are third
            }

            var betterLoser = new List<RoundStatEntity>() { loserA, loserB }.MinBy(x => x.EndPoints);
            return [betterLoser!];
        }

        throw new FlightsGameException("Third rank could not be determined!");
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