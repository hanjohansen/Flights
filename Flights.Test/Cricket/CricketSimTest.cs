using System.Diagnostics;
using Flights.Domain.Entities;
using Flights.Domain.Models;
using Xunit.Sdk;

namespace Flights.Test.Cricket;

public class CricketSimTest : CricketSimBase
{
    [Fact]
    public void RunSim(){
        for(int i = 1; i <= 1000; i++){
            Debug.WriteLine("running sim #" + i);
            SimulateGame();
        }
            
    }

    private void SimulateGame(){
        var players = _helpers.GetPlayers(4);

        var game = GameModel.Create(
            players, 
            GameType.Cricket,
            0,
            InOutModifier.None,
            InOutModifier.None);

        var state = game.SolveGameState();

        var darts = 0;

        while(darts < MaxGameDarts 
                && !state.Finished
                && state.CurrentPlayerId != null){

            var player = state.PlayerStates.First(x => x.PlayerId == state.CurrentPlayerId!.Value);
            var opts = GetDartOptions(player.CricketState!);
            var randValue = _helpers.GetRandom(opts);

            state = game.AddPlayerStats(state.CurrentPlayerId!.Value, StatModel.Init(randValue));

            darts++;            
        }

        if(!state.Finished){
            foreach(var stat in state.PlayerStates)
                Debug.WriteLine(stat.PlayerName + " " + stat.Points);
            throw new TestClassException("game did not finish!");
        }

        if(state.PlayerStates.Any(x => x.Rank == null))
            throw new TestClassException("ranking issue");

        Assert.True(true);
    }
}
