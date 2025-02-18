using Flights.Domain.Entities.Game;
using Flights.Domain.Models;

namespace Flights.Test.Tournament;

public class TournamentTest
{
    [Fact]
    public void FourPlayer_TournamentTest()
    {
        var testHelpers = new TestHelpers();
        var players = testHelpers.GetPlayers(4);

        var model = TournamentModel.Create(players, 2, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);
        
        Assert.True(model.Entity.Rounds.Count == 1);
        Assert.True(model.Entity.Rounds.Last().Games.Count == 2);
        Assert.Null(model.Entity.Rounds.Last().WildCard);
        
        //simulate finish of first game
        var firstGame = model.Entity.Rounds.Last().Games.First();
        testHelpers.FinishGame(firstGame.Game!);
        
        //resolve
        model.ResolveTournamentState();
        Assert.True(model.Entity.Rounds.Count == 1);
        
        //simulate finish of second game
        var secondGame = model.Entity.Rounds.Last().Games.Last();
        testHelpers.FinishGame(secondGame.Game!);
        
        //resolve
        model.ResolveTournamentState();
        Assert.True(model.Entity.Rounds.Count == 2);
        Assert.True(model.Entity.Rounds.Last().Games.Count == 1);
        
        //finish last game
        var lastGame = model.Entity.Rounds.Last().Games.Last();
        testHelpers.FinishGame(lastGame.Game!);
        
        //resolve
        model.ResolveTournamentState();
        Assert.True(model.Entity.Rounds.Count == 2);
        Assert.True(model.Entity.Rounds.Last().Games.Count == 1);

        Assert.NotNull(model.Entity.Finished);
    }
    
    [Fact]
    public void FivePlayer_TournamentTest()
    {
        var testHelpers = new TestHelpers();
        var players = testHelpers.GetPlayers(5);

        var model = TournamentModel.Create(players, 2, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);
        
        Assert.True(model.Entity.Rounds.Count == 1);
        Assert.True(model.Entity.Rounds.Last().Games.Count == 2);
        Assert.False(model.Entity.Rounds.Last().Games.Last().IsLosersCup);
        Assert.NotNull(model.Entity.Rounds.Last().Games.Last().Game);
        Assert.Null(model.Entity.Rounds.Last().WildCard);
        
        //simulate finish of regular games
        var firstGame = model.Entity.Rounds.Last().Games.First();
        var secondGame = model.Entity.Rounds.Last().Games[1];
        testHelpers.FinishGame(firstGame.Game!);
        testHelpers.FinishGame(secondGame.Game!);
        
        //resolve
        model.ResolveTournamentState();
        Assert.True(model.Entity.Rounds.Count == 2);
        Assert.NotNull(model.Entity.Rounds.Last().Games.Last().Game);
        
        //finish second round
        //finish last game
        var lastGame = model.Entity.Rounds.Last().Games[0];
        testHelpers.FinishGame(lastGame.Game!);
        
        //resolve
        model.ResolveTournamentState();
        Assert.True(model.Entity.Rounds.Count == 2);
        Assert.True(model.Entity.Rounds.Last().Games.Count == 1);
        Assert.NotNull(model.Entity.Finished);
    }
    
    [Fact]
    public void SixPlayer_TournamentTest()
    {
        var testHelpers = new TestHelpers();
        var players = testHelpers.GetPlayers(6);

        var model = TournamentModel.Create(players, 2, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);
        
        Assert.True(model.Entity.Rounds.Count == 1);
        Assert.True(model.Entity.Rounds.Last().Games.Count == 4);
        Assert.True(model.Entity.Rounds.Last().Games.Last().IsLosersCup);
        Assert.Null(model.Entity.Rounds.Last().Games.Last().Game);
        Assert.Null(model.Entity.Rounds.Last().WildCard);
        
        //simulate finish of regular games
        foreach(var game in model.Entity.Rounds.Last().Games)
            if(game.Game != null)
                testHelpers.FinishGame(game.Game);
        
        //resolve
        model.ResolveTournamentState();
        Assert.True(model.Entity.Rounds.Count == 1);
        Assert.NotNull(model.Entity.Rounds.Last().Games.Last().Game);
        
        //finish losers cup
        var loserCup = model.Entity.Rounds.Last().Games.Last();
        testHelpers.FinishGame(loserCup.Game!);
        
        //resolve
        model.ResolveTournamentState();
        Assert.True(model.Entity.Rounds.Count == 2);
        Assert.True(model.Entity.Rounds.Last().Games.Count == 2);
        
        //finish second round
        model.Entity.Rounds.Last().Games
            .ForEach(x =>
            {
                testHelpers.FinishGame(x.Game!);
            });
        
        //resolve
        model.ResolveTournamentState();
        Assert.True(model.Entity.Rounds.Count == 3);
        Assert.True(model.Entity.Rounds.Last().Games.Count == 1);

        //finish last game
        var lastGame = model.Entity.Rounds.Last().Games[0];
        testHelpers.FinishGame(lastGame.Game!);
        
        //resolve
        model.ResolveTournamentState();
        Assert.True(model.Entity.Rounds.Count == 3);
        Assert.True(model.Entity.Rounds.Last().Games.Count == 1);

        Assert.NotNull(model.Entity.Finished);
    }
    
        [Fact]
    public void SevenPlayer_TournamentTest()
    {
        var testHelpers = new TestHelpers();
        var players = testHelpers.GetPlayers(7);

        var model = TournamentModel.Create(players, 2, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);
        
        Assert.True(model.Entity.Rounds.Count == 1);
        Assert.True(model.Entity.Rounds.Last().Games.Count == 3);
        Assert.False(model.Entity.Rounds.Last().Games.Last().IsLosersCup);
        Assert.NotNull(model.Entity.Rounds.Last().Games.Last().Game);
        Assert.NotNull(model.Entity.Rounds.Last().WildCard);
        
        //simulate finish of regular games
        foreach(var game in model.Entity.Rounds.Last().Games)
            if(game.Game != null)
                testHelpers.FinishGame(game.Game);
        
        //resolve
        model.ResolveTournamentState();
        Assert.True(model.Entity.Rounds.Count == 2);
        Assert.True(model.Entity.Rounds.Last().Games.Count == 2);
        
        //simulate finish of games
        foreach(var game in model.Entity.Rounds.Last().Games)
                testHelpers.FinishGame(game.Game!);
        
        //resolve
        model.ResolveTournamentState();
        Assert.True(model.Entity.Rounds.Count == 3);
        Assert.True(model.Entity.Rounds.Last().Games.Count == 1);
        
        //finish third round
        var final = model.Entity.Rounds.Last().Games.Single();
        testHelpers.FinishGame(final.Game!);
        
        //resolve
        var state = model.ResolveTournamentState();
        Assert.True(model.Entity.Rounds.Count == 3);
        Assert.True(model.Entity.Rounds.Last().Games.Count == 1);

        Assert.NotNull(model.Entity.Finished);
    }
}