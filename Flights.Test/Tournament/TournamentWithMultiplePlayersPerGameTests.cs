using Flights.Domain.Entities.Game;
using Flights.Domain.Exception;
using Flights.Domain.Models;

namespace Flights.Test.Tournament;

public class TournamentWithMultiplePlayersPerGameTests
{
    private TestHelpers _helpers = new TestHelpers();
    
    [Fact]
    public void Five_Players_Three_Per_Game()
    {
        //five players, three per game => 3+2 
        var players = _helpers.GetPlayers(5);

        var model = TournamentModel.Create(players, 3, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);
        
        Assert.True(model.Entity.Rounds.Count == 1);
        Assert.True(model.Entity.Rounds.First().Games.Count == 2);

        var firstGame = model.Entity.Rounds.First().Games.First();
        Assert.True(firstGame.Game!.Players.Count == 3);

        var secGame = model.Entity.Rounds.First().Games[1];
        Assert.True(secGame.Game!.Players.Count == 2);
    }
    
    [Fact]
    public void Six_Players_Three_Per_Game()
    {
        //six players, three per game
        var players = _helpers.GetPlayers(6);

        var model = TournamentModel.Create(players, 3, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);
        
        Assert.True(model.Entity.Rounds.Count == 1);
        Assert.True(model.Entity.Rounds.First().Games.Count == 2);

        var firstGame = model.Entity.Rounds.First().Games.First();
        Assert.True(firstGame.Game!.Players.Count == 3);

        var secGame = model.Entity.Rounds.First().Games[1];
        Assert.True(secGame.Game!.Players.Count == 3);
    }
    
    [Fact]
    public void Seven_Players_Three_Per_Game()
    {
        //seven players, three per game => 3+3+wildcard
        var players = _helpers.GetPlayers(7);

        var model = TournamentModel.Create(players, 3, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);
        
        Assert.True(model.Entity.Rounds.Count == 1);
        Assert.True(model.Entity.Rounds.First().Games.Count == 3); //2 plus losers cup

        var firstGame = model.Entity.Rounds.First().Games.First();
        Assert.True(firstGame.Game!.Players.Count == 3);

        var secGame = model.Entity.Rounds.First().Games[1];
        Assert.True(secGame.Game!.Players.Count == 3);
        
        var thirdGame = model.Entity.Rounds.First().Games[2];
        Assert.True(thirdGame.IsLosersCup);
        
        Assert.True(model.Entity.Rounds.First().WildCard != null);
    }
    
    [Fact]
    public void Eight_Players_Three_Per_Game()
    {
        //eight players, three per game => 3+3+wildcard
        var players = _helpers.GetPlayers(8);

        var model = TournamentModel.Create(players, 3, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);
        
        Assert.True(model.Entity.Rounds.Count == 1);
        Assert.True(model.Entity.Rounds.First().Games.Count == 4); //3 plus losers cup

        var firstGame = model.Entity.Rounds.First().Games.First();
        Assert.True(firstGame.Game!.Players.Count == 3);

        var secGame = model.Entity.Rounds.First().Games[1];
        Assert.True(secGame.Game!.Players.Count == 3);
        
        var thirdGame = model.Entity.Rounds.First().Games[2];
        Assert.True(thirdGame.Game!.Players.Count == 2);
        
        var fourthGame = model.Entity.Rounds.First().Games[3];
        Assert.True(fourthGame.IsLosersCup);
        
        Assert.True(model.Entity.Rounds.First().WildCard == null);
    }
    
    [Fact]
    public void Nine_Players_Three_Per_Game()
    {
        //nine players, three per game => 3+3+wildcard
        var players = _helpers.GetPlayers(9);

        var model = TournamentModel.Create(players, 3, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);
        
        Assert.True(model.Entity.Rounds.Count == 1);
        Assert.True(model.Entity.Rounds.First().Games.Count == 4); //3 plus losers cup

        var firstGame = model.Entity.Rounds.First().Games.First();
        Assert.True(firstGame.Game!.Players.Count == 3);

        var secGame = model.Entity.Rounds.First().Games[1];
        Assert.True(secGame.Game!.Players.Count == 3);
        
        var thirdGame = model.Entity.Rounds.First().Games[2];
        Assert.True(thirdGame.Game!.Players.Count == 3);
        
        var fourthGame = model.Entity.Rounds.First().Games[3];
        Assert.True(fourthGame.IsLosersCup);
        
        Assert.True(model.Entity.Rounds.First().WildCard == null);
    }
    
    [Fact]
    public void Six_Players_Four_Per_Game()
    {
        //six players, four per game => 4+2
        var players = _helpers.GetPlayers(6);

        var model = TournamentModel.Create(players, 4, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);
        
        Assert.True(model.Entity.Rounds.Count == 1);
        Assert.True(model.Entity.Rounds.First().Games.Count == 2); 

        var firstGame = model.Entity.Rounds.First().Games.First();
        Assert.True(firstGame.Game!.Players.Count == 4);

        var secGame = model.Entity.Rounds.First().Games[1];
        Assert.True(secGame.Game!.Players.Count == 2);
    }
    
    [Fact]
    public void Seven_Players_Four_Per_Game()
    {
        //seven players, four per game => 4+3
        var players = _helpers.GetPlayers(7);

        var model = TournamentModel.Create(players, 4, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);
        
        Assert.True(model.Entity.Rounds.Count == 1);
        Assert.True(model.Entity.Rounds.First().Games.Count == 2); 

        var firstGame = model.Entity.Rounds.First().Games.First();
        Assert.True(firstGame.Game!.Players.Count == 4);

        var secGame = model.Entity.Rounds.First().Games[1];
        Assert.True(secGame.Game!.Players.Count == 3);
    }
    
    [Fact]
    public void Eight_Players_Four_Per_Game()
    {
        //eight players, four per game => 4+4
        var players = _helpers.GetPlayers(8);

        var model = TournamentModel.Create(players, 4, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);
        
        Assert.True(model.Entity.Rounds.Count == 1);
        Assert.True(model.Entity.Rounds.First().Games.Count == 2); 

        var firstGame = model.Entity.Rounds.First().Games.First();
        Assert.True(firstGame.Game!.Players.Count == 4);

        var secGame = model.Entity.Rounds.First().Games[1];
        Assert.True(secGame.Game!.Players.Count == 4);
    }
    
    [Fact]
    public void Nine_Players_Four_Per_Game()
    {
        //eight players, four per game => 4+4+loserscup
        var players = _helpers.GetPlayers(9);

        var model = TournamentModel.Create(players, 4, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);
        
        Assert.True(model.Entity.Rounds.Count == 1);
        Assert.True(model.Entity.Rounds.First().Games.Count == 3); 

        var firstGame = model.Entity.Rounds.First().Games.First();
        Assert.True(firstGame.Game!.Players.Count == 4);

        var secGame = model.Entity.Rounds.First().Games[1];
        Assert.True(secGame.Game!.Players.Count == 4);
        
        var thirdGame = model.Entity.Rounds.First().Games[2];
        Assert.True(thirdGame.IsLosersCup);
        
        Assert.True(model.Entity.Rounds.First().WildCard != null);
    }
    
    [Fact]
    public void ExceptionTest()
    {
        var players = _helpers.GetPlayers(4);

        Assert.Throws<FlightsGameException>(() =>
        {
            TournamentModel.Create(players, 3, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);
        });
        
        Assert.Throws<FlightsGameException>(() =>
        {                                                      //!!!
            TournamentModel.Create(players, 1, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);
        });
    }
}