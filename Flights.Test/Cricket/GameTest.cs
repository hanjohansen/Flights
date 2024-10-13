using Flights.Domain.Entities;
using Flights.Domain.Models;

namespace Flights.Test.Cricket;

public class GameTest
{
    private TestHelpers _helper = new TestHelpers();
    
    [Fact]
    public void GameStateReflectsRoundsCorrectly(){
        var players = _helper.GetPlayers(2);
    
        var model = GameModel.Create(
            players, 
            GameType.Cricket, 0, 
            InOutModifier.None, 
            InOutModifier.None);

        //round 1
        model.AddPlayerStats(
            players[0].Id,
            StatModel.Init(5),
            StatModel.Init(10),
            StatModel.Init(8));  //23

        var state = model.AddPlayerStats(
            players[1].Id,
            StatModel.Init(2),
            StatModel.Init(19),
            StatModel.Init(8));  //29

        //round 2
        // ...

        //round entity for second round does not exist 
        //unless first dart of new round is actually thrown
        //
        //state should still show new round 

        Assert.True(state.CurrentPlayerId != null);
        Assert.True(state.CurrentPlayerId == players[0].Id);
        Assert.True(state.Round == 2); //!!!

        model.AddPlayerStats(
            players[0].Id,
            StatModel.Init(5),
            StatModel.Init(10),
            StatModel.Init(8));  //23

        state = model.AddPlayerStats(
            players[1].Id,
            StatModel.Init(2),
            StatModel.Init(19),
            StatModel.Init(8));  //29

        Assert.True(state.Round == 3); //!!!
    }
}