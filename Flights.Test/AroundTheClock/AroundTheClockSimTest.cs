using Flights.Domain.Entities;
using Flights.Domain.Entities.Game;
using Flights.Domain.Models;

namespace Flights.Test.AroundTheClock;

public class AroundTheClockSimTest
{
    private readonly TestHelpers _helper = new();

    [Fact]
    public void RunSim(){
        for(var i = 0; i < 1000; i++){
            SimulateGame();
        }
    }

    private void SimulateGame(){
        var players = _helper.GetPlayers(3);
        var rand = new Random();
    
        var model = GameModel.Create(
            players, 
            GameType.AroundTheClock, 
            0, 
            InOutModifier.None, 
            InOutModifier.None);

        var state =  model.SolveGameState(); 

        for(var i = 0; i < 7; i++){
            var targetPoint = (i * 3) + 1;

            foreach(var unused in players){
                for(var h = 0; h < 3; h++){
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

        var rank = 1;
        var lastPoints = orderedPlayers[0].Points;
        foreach(var player in orderedPlayers){
            if(player.Points < lastPoints)
                rank++;

            Assert.True(player.Rank == rank);
            lastPoints = player.Points;
        }
    }

    private static int GetRandomPoint(int target, Random rand){
        var intTarget = target == 21
            ? 25
            : target;

        var randomPoint = rand.Next(intTarget -1, intTarget +1);

        return randomPoint == intTarget ? randomPoint : 0;
    }
}
