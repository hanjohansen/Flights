using Flights.Domain.Entities.Game;
using Flights.Domain.Exception;
using Flights.Domain.Models;

namespace Flights.Test.Tournament;

public class TournamentInitTest
{
    private TestHelpers _helpers = new TestHelpers();
    
    [Fact]
    public void InitLessThenFourPlayers()
    {
        var players = _helpers.GetPlayers(3);

        Assert.Throws<FlightsGameException>(() =>
            TournamentModel.Create(players, 2, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None));
    }

    [Fact]
    public void InitFivePlayers()
    {
        var players = _helpers.GetPlayers(5);

        var model = TournamentModel.Create(players, 2, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);
        
        Assert.True(model.Entity.Rounds.Count == 1);
        Assert.True(model.Entity.Rounds.First().Games.Count == 2);

        var firstGame = model.Entity.Rounds.First().Games.First();
        Assert.True(firstGame.Game!.Players.First().Player == players[0]);
        Assert.True(firstGame.Game.Players[1].Player == players[1]);
        Assert.True(firstGame.OrderNumber == 1);

        var secGame = model.Entity.Rounds.First().Games[1];
        Assert.True(secGame.Game!.Players.First().Player == players[2]);
        Assert.True(secGame.Game.Players[1].Player == players[3]);
        Assert.True(secGame.Game.Players[2].Player == players[4]);
        Assert.True(secGame.OrderNumber == 2);

        Assert.Null(model.Entity.Rounds.First().WildCard);
    }
    
    [Fact]
    public void InitFourPlayers()
    {
        var players = _helpers.GetPlayers(4);

        var model = TournamentModel.Create(players, 2, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);
        
        Assert.True(model.Entity.Rounds.Count == 1);
        Assert.True(model.Entity.Rounds.First().Games.Count == 2);

        var firstGame = model.Entity.Rounds.First().Games.First();
        Assert.True(firstGame.Game!.Players.First().Player == players[0]);
        Assert.True(firstGame.Game.Players[1].Player == players[1]);
        Assert.True(firstGame.OrderNumber == 1);

        var secGame = model.Entity.Rounds.First().Games[1];
        Assert.True(secGame.Game!.Players.First().Player == players[2]);
        Assert.True(secGame.Game.Players[1].Player == players[3]);
        Assert.True(secGame.OrderNumber == 2);
    }
    
    [Fact]
    public void InitSixPlayers()
    {
        var players = _helpers.GetPlayers(6);

        var model = TournamentModel.Create(players, 2, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);
        
        Assert.True(model.Entity.Rounds.Count == 1);
        Assert.True(model.Entity.Rounds.First().Games.Count == 4);

        var firstGame = model.Entity.Rounds.First().Games.First();
        Assert.True(firstGame.Game!.Players.First().Player == players[0]);
        Assert.True(firstGame.Game.Players[1].Player == players[1]);
        Assert.True(firstGame.OrderNumber == 1);

        var secGame = model.Entity.Rounds.First().Games[1];
        Assert.True(secGame.Game!.Players.First().Player == players[2]);
        Assert.True(secGame.Game.Players[1].Player == players[3]);
        Assert.True(secGame.OrderNumber == 2);
        
        var thirdGame = model.Entity.Rounds.First().Games[2];
        Assert.True(thirdGame.Game!.Players.First().Player == players[4]);
        Assert.True(thirdGame.Game.Players[1].Player == players[5]);
        Assert.True(thirdGame.OrderNumber == 3);
    }
    
    [Fact]
    public void InitSevenPlayers()
    {
        var players = _helpers.GetPlayers(7);

        var model = TournamentModel.Create(players, 2, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);
        
        Assert.True(model.Entity.Rounds.Count == 1);
        Assert.True(model.Entity.Rounds.First().Games.Count == 3);
        
        Assert.True(model.Entity.Rounds.First().WildCard != null);
        Assert.True(model.Entity.Rounds.First().WildCard!.Player == players.Last());

        var firstGame = model.Entity.Rounds.First().Games.First();
        Assert.True(firstGame.Game!.Players.First().Player == players[0]);
        Assert.True(firstGame.Game.Players[1].Player == players[1]);
        Assert.True(firstGame.OrderNumber == 1);

        var secGame = model.Entity.Rounds.First().Games[1];
        Assert.True(secGame.Game!.Players.First().Player == players[2]);
        Assert.True(secGame.Game.Players[1].Player == players[3]);
        Assert.True(secGame.OrderNumber == 2);
        
        var thirdGame = model.Entity.Rounds.First().Games[2];
        Assert.True(thirdGame.Game!.Players.First().Player == players[4]);
        Assert.True(thirdGame.Game.Players[1].Player == players[5]);
        Assert.True(thirdGame.OrderNumber == 3);
    }
}