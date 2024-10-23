using Flights.Domain.Entities;
using Flights.Domain.Models;

namespace Flights.Test.Shanghai;

public class ShanghaiSimTest
{
    private TestHelpers _helper = new TestHelpers();

    [Fact]
    public void RunSim(){
        for(int i = 0; i < 1000; i++){
            SimulateGame();
        }
    }

    private void SimulateGame(){
        var players = _helper.GetPlayers(3);
        var rand = new Random();
    
        var model = GameModel.Create(
            players, 
            GameType.Shanghai, 
            0, 
            InOutModifier.None, 
            InOutModifier.None);

        var state =  model.SolveGameState(); 

        for(int i = 0; i < 7; i++){
            var targetPoint = (i * 3) + 1;

            foreach(var player in players){
                for(int h = 0; h < 3; h++){
                    var intTarget = targetPoint + h;
                    var randPoint = GetRandomPoint(intTarget, rand);

                    state = model.AddPlayerStats(state.CurrentPlayerId!.Value, StatModel.Init(randPoint));
                }
            }
        }

        Assert.True(state.Finished);

        foreach(var player in state.PlayerStates){
            Assert.True(player.Rank != null);
        }

        var orderedPlayers = state.PlayerStates.OrderByDescending(x => x.Points).ToList();

        int rank = 1;
        var lastPoints = orderedPlayers[0].Points;
        foreach(var player in orderedPlayers){
            if(player.Points < lastPoints)
                rank++;

            Assert.True(player.Rank == rank);
            lastPoints = player.Points;
        }
    }

    private int GetRandomPoint(int target, Random rand){
        var intTarget = target == 21
            ? 25
            : target;

        var randomPoint = rand.Next(intTarget -1, intTarget +1);

        if(randomPoint == intTarget)
            return randomPoint;

        return 0;
    }
}
