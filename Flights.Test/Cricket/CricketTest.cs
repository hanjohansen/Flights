using Flights.Domain.Entities;
using Flights.Domain.Models;

namespace Flights.Test.Cricket;
public class CricketTest
{
    private TestHelpers _helper = new TestHelpers();

    [Fact]
    public void Initializes_Game_Correctly(){
        var players = _helper.GetPlayers(3);
        
        var model = GameModel.Create(
            players, 
            GameType.Cricket, 
            0, 
            InOutModifier.None, 
            InOutModifier.None);

        var state = model.SolveGameState(); 

        Assert.False(state.Finished);
        Assert.True(state.InModifier == InOutModifier.None);
        Assert.True(state.OutModifier == InOutModifier.None);

        foreach(var player in state.PlayerStates){
            Assert.True(player.IsIn);
            Assert.False(player.IsBust);
            Assert.True(player.Points == 0);
            Assert.Null(player.Rank);
        }
    }


    [Fact]
    public void Game_Starts_Correctly(){
        var players = _helper.GetPlayers(3);
        
        var model = GameModel.Create(
            players, 
            GameType.Cricket, 
            0, 
            InOutModifier.None, 
            InOutModifier.None);

        var state = model.SolveGameState(); 

        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.None, 15));
        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.Double, 16));
        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.Triple, 17));

        state = model.SolveGameState();

        Assert.True(state.CricketState!.V15 == Domain.State.CricketValue.Single);
        Assert.True(state.CricketState!.V16 == Domain.State.CricketValue.Double);
        Assert.True(state.CricketState!.V17 == Domain.State.CricketValue.Open);

        var playerState = state.PlayerStates.First(x => x.PlayerId == players[0].Id);

        Assert.True(playerState.CricketState!.V15 == Domain.State.CricketValue.Single);
        Assert.True(playerState.CricketState!.V16 == Domain.State.CricketValue.Double);
        Assert.True(playerState.CricketState!.V17 == Domain.State.CricketValue.Open);
    }

    [Fact]
    public void Game_Closes_Value_Correctly(){
        var players = _helper.GetPlayers(3);
        
        var model = GameModel.Create(
            players, 
            GameType.Cricket, 
            0, 
            InOutModifier.None, 
            InOutModifier.None);

        var state = model.SolveGameState(); 

        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.None, 15));
        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.Double, 16));
        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.Triple, 17));

        model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.None, 0));
        model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.None, 0));
        model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.Triple, 17));

        model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.None, 15));
        model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.None, 0));
        model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.Triple, 17));

        state = model.SolveGameState();

        Assert.True(state.CricketState!.V15 == Domain.State.CricketValue.Single);
        Assert.True(state.CricketState!.V16 == Domain.State.CricketValue.Double);
        Assert.True(state.CricketState!.V17 == Domain.State.CricketValue.Closed);

        foreach(var player in state.PlayerStates)
            Assert.True(player.CricketState!.V17 == Domain.State.CricketValue.Closed);
    }
}