using System.Diagnostics;
using Flights.Domain.Entities;
using Flights.Domain.Models;

namespace Flights.Test.Cricket;

public class CricketSimTest : CricketSimBase
{
    [Fact]
    public void RunSim(){
        for(var i = 1; i <= SimRounds; i++){
            Debug.WriteLine("running sim #" + i);
            SimulateGame();
        }
    }

    private void SimulateGame(){
        var players = Helpers.GetPlayers(4);

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
            var others = state.PlayerStates.Where(x => x.PlayerId != player.PlayerId)
                .Select(x => x.CricketState!)
                .ToList();
            var opts = GetDartOptions(player.CricketState!, others);
            var randValue = Helpers.GetRandom(opts);

            state = game.AddPlayerStats(state.CurrentPlayerId!.Value, StatModel.Init(randValue));
            darts++;            
        }
        
        DoFinishAsserts(state);
    }
}
