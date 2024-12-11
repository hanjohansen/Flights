using System.Diagnostics;
using Flights.Domain.Entities.Game;
using Flights.Domain.Models;

namespace Flights.Test.Cricket;

public class CricketSimTest : CricketSimBase
{
    [Fact]
    public void RunCricketSim(){
        var players = Helpers.GetPlayers(4);
        
        for(var i = 1; i <= SimRounds; i++){
            Debug.WriteLine("running sim #" + i);
            
            var game = GameModel.Create(
                players, 
                GameType.Cricket,
                false,
                0,
                InOutModifier.None,
                InOutModifier.None);
            
            SimulateGame(game);
        }
    }
    
    [Fact]
    public void RunCricketSim_QuickFinish(){
        var players = Helpers.GetPlayers(4);
        
        for(var i = 1; i <= SimRounds; i++){
            Debug.WriteLine("running sim #" + i);
            
            var game = GameModel.Create(
                players, 
                GameType.Cricket,
                true,
                0,
                InOutModifier.None,
                InOutModifier.None);
            
            SimulateGame(game);
        }
    }
    
    [Fact]
    public void RunCtCricketSim(){
        var players = Helpers.GetPlayers(4);
        
        for(var i = 1; i <= SimRounds; i++){
            Debug.WriteLine("running sim #" + i);
            
            var game = GameModel.Create(
                players, 
                GameType.CtCricket,
                false,
                0,
                InOutModifier.None,
                InOutModifier.None);
            
            SimulateGame(game);
        }
    }
    
    [Fact]
    public void RunCtCricketSim_QuickFinish(){
        var players = Helpers.GetPlayers(4);
        
        for(var i = 1; i <= SimRounds; i++){
            Debug.WriteLine("running sim #" + i);
            
            var game = GameModel.Create(
                players, 
                GameType.CtCricket,
                true,
                0,
                InOutModifier.None,
                InOutModifier.None);
            
            SimulateGame(game);
        }
    }

    private void SimulateGame(GameModel game){
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
