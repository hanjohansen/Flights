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

        Assert.True(state.Type == GameType.Cricket);
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

    [Fact]
    public void Game_Sets_Points_Correctly(){
        var players = _helper.GetPlayers(3);
        
        var model = GameModel.Create(
            players, 
            GameType.Cricket, 
            0, 
            InOutModifier.None, 
            InOutModifier.None);

        var state = model.SolveGameState(); 

        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.None, 0));
        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.None, 0));
        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.Triple, 17));

        model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.None, 0));
        model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.None, 0));
        model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.Triple, 17));

        model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.None, 0));
        model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.None, 0));
        model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.None, 0));

        //p1 + p2 have 17 closed, p3 
        state = model.SolveGameState();

        Assert.True(state.CricketState!.V17 == Domain.State.CricketValue.Open);

        foreach(var player in players){
            var playerState = state.PlayerStates.First(x => x.PlayerId == player.Id);

            if(players.IndexOf(player) <2)
                Assert.True(playerState.CricketState!.V17 == Domain.State.CricketValue.Open);
            else
                Assert.True(playerState.CricketState!.V17 == Domain.State.CricketValue.None);
        }

        //points on 17 should yield points for p1 + p2
        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.None, 0));
        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.None, 0));
        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.Triple, 17));  //!!!

        model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.None, 0));
        model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.None, 0));
        model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.Double, 17));  //!!!

        model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.None, 0));
        model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.None, 0));
        model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.None, 0));

        //p1 + p2 have points?
        state = model.SolveGameState();

        foreach(var player in players){
            var playerState = state.PlayerStates.First(x => x.PlayerId == player.Id);

            if(players.IndexOf(player) == 0)
                Assert.True(playerState.Points == 17 * 3);
            if(players.IndexOf(player) == 1)
                Assert.True(playerState.Points == 17 * 2);
            if(players.IndexOf(player) == 2)
                Assert.True(playerState.Points == 0);
        }
    }

    [Fact]
    public void Game_Finishes_Simple_Correctly(){
        var players = _helper.GetPlayers(3);
        
        var model = GameModel.Create(
            players, 
            GameType.Cricket, 
            0, 
            InOutModifier.None, 
            InOutModifier.None);

        var state = model.SolveGameState(); 

        //first round
        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.Triple, 15), 
            StatModel.Init(DartModifier.Triple, 16), 
            StatModel.Init(DartModifier.Triple, 17));

        model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.Triple, 15), 
            StatModel.Init(DartModifier.Triple, 16), 
            StatModel.Init(DartModifier.Triple, 17));

        model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.Triple, 15), 
            StatModel.Init(DartModifier.Triple, 16), 
            StatModel.Init(DartModifier.Triple, 17));

        //second round
        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.Triple, 18), 
            StatModel.Init(DartModifier.Triple, 19), 
            StatModel.Init(DartModifier.Triple, 20));

        model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.Triple, 18), 
            StatModel.Init(DartModifier.Triple, 19), 
            StatModel.Init(DartModifier.Triple, 20));

        model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.Triple, 18), 
            StatModel.Init(DartModifier.Triple, 19), 
            StatModel.Init(DartModifier.Triple, 20));

        //third round
        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.Double, 25));

        state = model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.None, 25));

        //p1 should have won the game now
        var playerState = state.PlayerStates.First(x => x.PlayerId == players[0].Id);

        Assert.True(state.PlayerStates[0].Rank == 1);
        Assert.True(state.PlayerStates[1].Rank == null);
        Assert.True(state.PlayerStates[2].Rank == null);

        //next player should now be p2
        Assert.True(state.CurrentPlayerId == players[1].Id);

        //adding new dart should keep ranking
        state = model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.Double, 25));

        Assert.True(state.PlayerStates[0].Rank == 1);
        Assert.True(state.PlayerStates[1].Rank == null);
        Assert.True(state.PlayerStates[2].Rank == null);

        Assert.True(state.CurrentPlayerId == players[1].Id);

        //p2 throws winning dart
        state = model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.None, 25));

        Assert.True(state.PlayerStates[0].Rank == 1);
        Assert.True(state.PlayerStates[1].Rank == 2);
        Assert.True(state.PlayerStates[2].Rank == 3);

        Assert.True(state.CurrentPlayerId == null);
    }
}