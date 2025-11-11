using Flights.Domain.Entities.Game;
using Flights.Domain.Models;
using Flights.Domain.State;

namespace Flights.Test.Tournament;

public class TournamentShuffleRoundTest
{
    [Fact]
    public void ShuffleFirstRoundTest()
    {
        var testHelpers = new TestHelpers();
        var players = testHelpers.GetPlayers(7);

        var model = TournamentModel.Create(Guid.Empty, players, 2, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);
        
        Assert.True(model.Entity.Rounds.Count == 1);
        Assert.True(model.Entity.Rounds.Last().Games.Count == 3);
        Assert.False(model.Entity.Rounds.Last().Games.Last().IsLosersCup);
        Assert.NotNull(model.Entity.Rounds.Last().Games.Last().Game);
        Assert.NotNull(model.Entity.Rounds.Last().WildCard);
        
        var round =  model.Entity.Rounds.Last();
        Assert.False(round.HasDarts());
        
        //cache player indexes
        var playerIndex = new List<Guid>();
        
        foreach(var roundGame in round.Games)
            foreach(var player in roundGame.Game!.Players)
                playerIndex.Add(player.Player.Id);
        
        //act
        model.ReshuffleLastRound();
        
        //cache new indexes
        round = model.Entity.Rounds.Last();
        var newPlayerIndex = new List<Guid>();
        
        foreach(var roundGame in round.Games)
            foreach(var player in roundGame.Game!.Players)
                newPlayerIndex.Add(player.Player.Id);

        var results = new List<bool>();
        foreach (var player in playerIndex)
        {
            var index = playerIndex.IndexOf(player);
            var other = newPlayerIndex[index];
            
            results.Add(player != other);
        }

        var shuffledSomth = results.Any(x => x);
        Assert.True(shuffledSomth);
    }
    
    [Fact]
    public void ShuffleNonFirstRoundTest()
    {
        var testHelpers = new TestHelpers();
        var players = testHelpers.GetPlayers(8);

        var model = TournamentModel.Create(Guid.Empty, players, 2, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);
        
        Assert.True(model.Entity.Rounds.Count == 1);
        Assert.True(model.Entity.Rounds.Last().Games.Count == 4);
        Assert.False(model.Entity.Rounds.Last().Games.Last().IsLosersCup);
        Assert.NotNull(model.Entity.Rounds.Last().Games.Last().Game);
        Assert.Null(model.Entity.Rounds.Last().WildCard);

        var solver = GameSolverFactory.GetTournamentSolver(model.Entity);

        //simulate finish of regular games
        foreach(var game in model.Entity.Rounds.Last().Games)
            if(game.Game != null)
                testHelpers.FinishGame(game.Game);
        
        //init second round
        solver.Solve();
        Assert.True(model.Entity.Rounds.Count == 2);
        
        var round =  model.Entity.Rounds.Last();
        Assert.False(round.HasDarts());
        
        //cache player indexes
        var playerIndex = new List<Guid>();
        
        foreach(var roundGame in round.Games)
            foreach(var player in roundGame.Game!.Players)
                playerIndex.Add(player.Player.Id);
        
        //act
        model.ReshuffleLastRound();
        
        //cache new indexes
        round = model.Entity.Rounds.Last();
        var newPlayerIndex = new List<Guid>();
        
        foreach(var roundGame in round.Games)
            foreach(var player in roundGame.Game!.Players)
                newPlayerIndex.Add(player.Player.Id);

        var results = new List<bool>();
        foreach (var player in playerIndex)
        {
            var index = playerIndex.IndexOf(player);
            var other = newPlayerIndex[index];
            
            results.Add(player != other);
        }

        var shuffledSomth = results.Any(x => x);
        Assert.True(shuffledSomth);
    }
}