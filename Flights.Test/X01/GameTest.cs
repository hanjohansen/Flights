
using Flights.Domain.Entities;
using Flights.Domain.Models;

namespace Flights.Test.X01;

public class GameTest
{
    private TestHelpers _helper = new TestHelpers();

    [Fact]
    public void PlayerRoundTest(){
        var players = _helper.GetPlayers(4);
    
        var model = GameModel.Create(
            players, 
            GameType.X01, 301, 
            InOutModifier.None, 
            InOutModifier.None);

        //round 1
        model.AddPlayerStats(
            players[0].Id,
            StatModel.Init(5),
            StatModel.Init(10),
            StatModel.Init(8));  //23

        model.AddPlayerStats(
            players[1].Id,
            StatModel.Init(2),
            StatModel.Init(19),
            StatModel.Init(8));  //29

        model.AddPlayerStats(
            players[2].Id,
            StatModel.Init(2),
            StatModel.Init(19),
            StatModel.Init(8));  //29

        var state = model.AddPlayerStats(
            players[3].Id,
            StatModel.Init(2),
            StatModel.Init(19),
            StatModel.Init(8));  //29

        Assert.True(state.CurrentPlayerId != null);
        Assert.True(state.CurrentPlayerId == players[0].Id);
    }

    [Fact]
    public void PlayerRoundSecondTest(){
        var players = _helper.GetPlayers(4);
    
        var model = GameModel.Create(
            players, 
            GameType.X01, 301, 
            InOutModifier.None, 
            InOutModifier.None);

        //round 1
        model.AddPlayerStats(
            players[0].Id,
            StatModel.Init(5),
            StatModel.Init(10),
            StatModel.Init(8));  //23

        model.AddPlayerStats(
            players[1].Id,
            StatModel.Init(2),
            StatModel.Init(19),
            StatModel.Init(8));  //29

        model.AddPlayerStats(
            players[2].Id,
            StatModel.Init(2),
            StatModel.Init(19),
            StatModel.Init(8));  //29

        model.AddPlayerStats(players[3].Id, StatModel.Init(2));
        model.AddPlayerStats(players[3].Id, StatModel.Init(19));
        var state = model.AddPlayerStats(players[3].Id, StatModel.Init(8));  //29

        Assert.True(state.CurrentPlayerId != null);
        Assert.True(state.CurrentPlayerId == players[0].Id);
    }
}
