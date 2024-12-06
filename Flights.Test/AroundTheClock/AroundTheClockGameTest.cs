using Flights.Domain.Entities;
using Flights.Domain.Entities.Game;
using Flights.Domain.Models;

namespace Flights.Test.AroundTheClock;

public class AroundTheClockGameTest
{
    private readonly TestHelpers _helper = new();

    [Fact]
    public void Initializes_Game_Correctly(){
        var players = _helper.GetPlayers(2);
            
        var model = GameModel.Create(
            players, 
            GameType.AroundTheClock, 0, 
            InOutModifier.None, 
            InOutModifier.None);

        var state = model.SolveGameState(); 

        Assert.False(state.Finished);
        Assert.True(state.InModifier == InOutModifier.None);
        Assert.True(state.OutModifier == InOutModifier.None);

        Assert.True(state.AroundTheClockState!.CurrentTarget == 1);
        Assert.True(state.CurrentPlayerId == state.PlayerStates.First().PlayerId);

        foreach(var player in state.PlayerStates){
            Assert.True(player.IsIn);
            Assert.False(player.IsBust);
            Assert.True(player.Points == 0);
            Assert.Null(player.Rank);
            Assert.True(player.AroundTheClockState!.CurrentTarget == 1);
        }
    }
        
    [Fact]
    public void Game_Starts_Correctly(){
        var players = _helper.GetPlayers(3);
            
        var model = GameModel.Create(
            players, 
            GameType.AroundTheClock, 
            0, 
            InOutModifier.None, 
            InOutModifier.None);

        var state =  model.SolveGameState(); 

        //first player 1-3

        Assert.True(state.AroundTheClockState!.CurrentTarget == 1);

        state =  model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.None, 1));

        Assert.True(state.AroundTheClockState!.CurrentTarget == 2);    

        state = model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.Double, 2));

        Assert.True(state.AroundTheClockState!.CurrentTarget == 3);

        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.Triple, 3));

        state = model.SolveGameState();

        var playerState = state.PlayerStates.First(x => x.PlayerId == players[0].Id);

        Assert.True(playerState.AroundTheClockState!.V1 == 1);
        Assert.True(playerState.AroundTheClockState!.V2 == 4);
        Assert.True(playerState.AroundTheClockState!.V3 == 9);

        Assert.True(playerState.AroundTheClockState!.CurrentTarget == 3);
        Assert.True(state.CurrentPlayerId == state.PlayerStates[1].PlayerId);
        Assert.True(state.AroundTheClockState!.CurrentTarget == 1);

        //second player 1-3

        state =  model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.None, 0));

        Assert.True(state.AroundTheClockState!.CurrentTarget == 2);    

        state = model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.Triple, 2));

        Assert.True(state.AroundTheClockState!.CurrentTarget == 3);

        model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.None, 3));

        state = model.SolveGameState();

        playerState = state.PlayerStates.First(x => x.PlayerId == players[1].Id);

        Assert.True(playerState.AroundTheClockState!.V1 == 0);
        Assert.True(playerState.AroundTheClockState!.V2 == 6);
        Assert.True(playerState.AroundTheClockState!.V3 == 3);

        Assert.True(playerState.AroundTheClockState!.CurrentTarget == 3);
        Assert.True(state.CurrentPlayerId == state.PlayerStates[2].PlayerId);
        Assert.True(state.AroundTheClockState!.CurrentTarget == 1);

        //third player 1-3

        state =  model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.None, 0));

        Assert.True(state.AroundTheClockState!.CurrentTarget == 2);    

        state = model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.None, 0));

        Assert.True(state.AroundTheClockState!.CurrentTarget == 3);

        model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.Triple, 3));

        state = model.SolveGameState();

        playerState = state.PlayerStates.First(x => x.PlayerId == players[2].Id);

        Assert.True(playerState.AroundTheClockState!.V1 == 0);
        Assert.True(playerState.AroundTheClockState!.V2 == 0);
        Assert.True(playerState.AroundTheClockState!.V3 == 9);

        Assert.True(state.CurrentPlayerId == state.PlayerStates[0].PlayerId);
        Assert.True(state.AroundTheClockState!.CurrentTarget == 4);


        // second round

        //first player 4-6

        state =  model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.None, 0));

        Assert.True(state.AroundTheClockState!.CurrentTarget == 5);    

        state = model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.Double, 5));

        Assert.True(state.AroundTheClockState!.CurrentTarget == 6);

        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.None, 0));

        state = model.SolveGameState();

        playerState = state.PlayerStates.First(x => x.PlayerId == players[0].Id);

        Assert.True(playerState.AroundTheClockState!.V4 == 0);
        Assert.True(playerState.AroundTheClockState!.V5 == 10);
        Assert.True(playerState.AroundTheClockState!.V6 == 0);

        Assert.True(playerState.AroundTheClockState!.CurrentTarget == 6);
        Assert.True(state.CurrentPlayerId == state.PlayerStates[1].PlayerId);
        Assert.True(state.AroundTheClockState!.CurrentTarget == 4);

        //second player 4-6

        state =  model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.None, 0));

        Assert.True(state.AroundTheClockState!.CurrentTarget == 5);    

        state = model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.None, 0));

        Assert.True(state.AroundTheClockState!.CurrentTarget == 6);

        model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.Triple, 6));

        state = model.SolveGameState();

        playerState = state.PlayerStates.First(x => x.PlayerId == players[1].Id);

        Assert.True(playerState.AroundTheClockState!.V4 == 0);
        Assert.True(playerState.AroundTheClockState!.V5 == 0);
        Assert.True(playerState.AroundTheClockState!.V6 == 18);

        Assert.True(state.CurrentPlayerId == state.PlayerStates[2].PlayerId);
        Assert.True(state.AroundTheClockState!.CurrentTarget == 4);

        //third player 4-6

        state =  model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.Double, 4));

        Assert.True(state.AroundTheClockState!.CurrentTarget == 5);    

        state = model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.None, 0));

        Assert.True(state.AroundTheClockState!.CurrentTarget == 6);

        model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.None, 0));

        state = model.SolveGameState();

        playerState = state.PlayerStates.First(x => x.PlayerId == players[2].Id);

        Assert.True(playerState.AroundTheClockState!.V4 == 8);
        Assert.True(playerState.AroundTheClockState!.V5 == 0);
        Assert.True(playerState.AroundTheClockState!.V6 == 0);

        Assert.True(state.CurrentPlayerId == state.PlayerStates[0].PlayerId);
        Assert.True(state.AroundTheClockState!.CurrentTarget == 7);

        // third round

        //first player 7-9

        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.Double, 7));  

        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.None, 0));

        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.None, 0));

        state = model.SolveGameState();

        playerState = state.PlayerStates.First(x => x.PlayerId == players[0].Id);

        Assert.True(playerState.AroundTheClockState!.V7 == 14);
        Assert.True(playerState.AroundTheClockState!.V8 == 0);
        Assert.True(playerState.AroundTheClockState!.V9 == 0);

        Assert.True(state.CurrentPlayerId == state.PlayerStates[1].PlayerId);
        Assert.True(state.AroundTheClockState!.CurrentTarget == 7);

        //second player 7-9

        model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.None, 0)); 

        model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.None, 0));

        model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.Double, 9));

        state = model.SolveGameState();

        playerState = state.PlayerStates.First(x => x.PlayerId == players[1].Id);

        Assert.True(playerState.AroundTheClockState!.V7 == 0);
        Assert.True(playerState.AroundTheClockState!.V8 == 0);
        Assert.True(playerState.AroundTheClockState!.V9 == 18);

        Assert.True(state.CurrentPlayerId == state.PlayerStates[2].PlayerId);
        Assert.True(state.AroundTheClockState!.CurrentTarget == 7);

        //third player 7-9

        model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.None, 0));   

        model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.Triple, 8));

        model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.None, 0));

        state = model.SolveGameState();

        playerState = state.PlayerStates.First(x => x.PlayerId == players[2].Id);

        Assert.True(playerState.AroundTheClockState!.V7 == 0);
        Assert.True(playerState.AroundTheClockState!.V8 == 24);
        Assert.True(playerState.AroundTheClockState!.V9 == 0);

        Assert.True(state.CurrentPlayerId == state.PlayerStates[0].PlayerId);
        Assert.True(state.AroundTheClockState!.CurrentTarget == 10);
    }

    [Fact]
    public void First_Player_Starts_Second_Round_With_Zero_Darts()
    {
        var players = _helper.GetPlayers(3);

        var model = GameModel.Create(
            players,
            GameType.AroundTheClock,
            0,
            InOutModifier.None,
            InOutModifier.None);

        var state = model.SolveGameState();

        foreach (var player in state.PlayerStates)
            Assert.True(player.Darts?.GetDartsList().Count == 0);

        //first round
        model.AddPlayerStats(players[0].Id,
            StatModel.Init(DartModifier.None, 1),
            StatModel.Init(DartModifier.None, 2),
            StatModel.Init(DartModifier.None, 3));

        model.AddPlayerStats(players[1].Id,
            StatModel.Init(DartModifier.None, 1),
            StatModel.Init(DartModifier.None, 2),
            StatModel.Init(DartModifier.None, 3));

        state = model.AddPlayerStats(players[2].Id,
            StatModel.Init(DartModifier.None, 1),
            StatModel.Init(DartModifier.None, 2),
            StatModel.Init(DartModifier.None, 3)); //second round starts

        Assert.True(state.PlayerStates[0].Darts!.GetDartsList().Count == 0);

        state = model.AddPlayerStats(players[0].Id,
            StatModel.Init(DartModifier.None, 4));
        
        Assert.True(state.PlayerStates[0].Darts!.GetDartsList().Count == 1);
    }
}