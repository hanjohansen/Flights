using Flights.Domain.Entities.Game;
using Flights.Domain.Models;
using Flights.Domain.State;

namespace Flights.Test.Cricket;
public class CricketTest
{
    private readonly TestHelpers _helper = new();

    [Fact]
    public void Initializes_Game_Correctly(){
        var players = _helper.GetPlayers(3);
        
        var model = GameModel.Create(
            players, 
            GameType.Cricket, 
            false,
            0, 
            InOutModifier.None, 
            InOutModifier.None);

        var state = model.SolveGameState(); 

        Assert.True(state.Type == GameType.Cricket);
        Assert.False(state.Finished);
        Assert.True(state.InModifier == InOutModifier.None);
        Assert.True(state.OutModifier == InOutModifier.None);

        Assert.True(state.CurrentPlayerId == state.PlayerStates.First().PlayerId);

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
            false,
            0, 
            InOutModifier.None, 
            InOutModifier.None);

        model.SolveGameState(); 

        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.None, 15));
        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.Double, 16));
        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.Triple, 17));

        var state = model.SolveGameState();

        Assert.True(state.CricketState!.V15 == CricketValue.Single);
        Assert.True(state.CricketState!.V16 == CricketValue.Double);
        Assert.True(state.CricketState!.V17 == CricketValue.Open);

        var playerState = state.PlayerStates.First(x => x.PlayerId == players[0].Id);

        Assert.True(playerState.CricketState!.V15 == CricketValue.Single);
        Assert.True(playerState.CricketState!.V16 == CricketValue.Double);
        Assert.True(playerState.CricketState!.V17 == CricketValue.Open);
    }

    [Fact]
    public void Game_Closes_Value_Correctly(){
        var players = _helper.GetPlayers(3);
        
        var model = GameModel.Create(
            players, 
            GameType.Cricket, 
            false,
            0, 
            InOutModifier.None, 
            InOutModifier.None);

        model.SolveGameState(); 

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

        var state = model.SolveGameState();

        Assert.True(state.CricketState!.V15 == CricketValue.Single);
        Assert.True(state.CricketState!.V16 == CricketValue.Double);
        Assert.True(state.CricketState!.V17 == CricketValue.Closed);

        foreach(var player in state.PlayerStates)
            Assert.True(player.CricketState!.V17 == CricketValue.Closed);
    }

    [Fact]
    public void Game_Sets_Points_Correctly(){
        var players = _helper.GetPlayers(3);
        
        var model = GameModel.Create(
            players, 
            GameType.Cricket, 
            false,
            0, 
            InOutModifier.None, 
            InOutModifier.None);

        model.SolveGameState(); 

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
        var state = model.SolveGameState();

        Assert.True(state.CricketState!.V17 == CricketValue.Open);

        foreach(var player in players){
            var playerState = state.PlayerStates.First(x => x.PlayerId == player.Id);

            if(players.IndexOf(player) <2)
                Assert.True(playerState.CricketState!.V17 == CricketValue.Open);
            else
                Assert.True(playerState.CricketState!.V17 == CricketValue.None);
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
            false,
            0, 
            InOutModifier.None, 
            InOutModifier.None);

        model.SolveGameState(); 

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

        var state = model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.None, 25));

        //p1 should have won the game now
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

    [Fact]
    public void Game_Finishes_Complex_Correctly(){
        var players = _helper.GetPlayers(4);
        
        var model = GameModel.Create(
            players, 
            GameType.Cricket, 
            false,
            0, 
            InOutModifier.None, 
            InOutModifier.None);

        model.SolveGameState(); 

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

        model.AddPlayerStats(players[3].Id, 
            StatModel.Init(DartModifier.Triple, 15), 
            StatModel.Init(DartModifier.Triple, 16), 
            StatModel.Init(DartModifier.Triple, 17));

        //second round
        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.Triple, 18), 
            StatModel.Init(DartModifier.Triple, 19), 
            StatModel.Init(0));

        model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.Triple, 18), 
            StatModel.Init(DartModifier.Triple, 19), 
            StatModel.Init(0));

        model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.Triple, 18), 
            StatModel.Init(DartModifier.Triple, 19), 
            StatModel.Init(0));

        var state = model.AddPlayerStats(players[3].Id, 
            StatModel.Init(DartModifier.Triple, 18), 
            StatModel.Init(DartModifier.Triple, 19), 
            StatModel.Init(0));

        //     p1  p2  p3  p4
        //      0   0   0   0
        //
        // 15   0   0   0   0
        // 16   0   0   0   0
        // 17   0   0   0   0
        // 18   0   0   0   0
        // 19   0   0   0   0
        // 20   -   -   -   -
        // B    -   -   -   -

        foreach(var player in state.PlayerStates){
            Assert.True(player.CricketState!.V15 == CricketValue.Closed);
            Assert.True(player.CricketState!.V16 == CricketValue.Closed);
            Assert.True(player.CricketState!.V17 == CricketValue.Closed);
            Assert.True(player.CricketState!.V18 == CricketValue.Closed);
            Assert.True(player.CricketState!.V19 == CricketValue.Closed);
            Assert.True(player.CricketState!.V20 == CricketValue.None);
            Assert.True(player.CricketState!.Bulls == CricketValue.None);
            Assert.True(player.Points == 0);
        }

        //third round
        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.Double, 20), // !!!!!!!!
            StatModel.Init(0), 
            StatModel.Init(0));

        model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.Triple, 20), 
            StatModel.Init(0), 
            StatModel.Init(0));

        model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.Triple, 20), 
            StatModel.Init(0), 
            StatModel.Init(0));

        state = model.AddPlayerStats(players[3].Id, 
            StatModel.Init(DartModifier.Triple, 20), 
            StatModel.Init(0), 
            StatModel.Init(0));

        //     p1  p2  p3  p4
        //      0   0   0   0
        //
        // 15   0   0   0   0
        // 16   0   0   0   0
        // 17   0   0   0   0
        // 18   0   0   0   0
        // 19   0   0   0   0
        // 20   x   Q   Q   Q
        // B    -   -   -   -

        Assert.True(state.CricketState!.V20 == CricketValue.Open);

        foreach(var player in state.PlayerStates){
            Assert.True(player.CricketState!.V15 == CricketValue.Closed);
            Assert.True(player.CricketState!.V16 == CricketValue.Closed);
            Assert.True(player.CricketState!.V17 == CricketValue.Closed);
            Assert.True(player.CricketState!.V18 == CricketValue.Closed);
            Assert.True(player.CricketState!.V19 == CricketValue.Closed);
            //Assert.True(player.CricketState!.V20 == Domain.State.CricketValue.None);
            Assert.True(player.CricketState!.Bulls == CricketValue.None);
            Assert.True(player.Points == 0);
        }

        //fourth round
        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(0), // !!!!!!!!
            StatModel.Init(0), 
            StatModel.Init(0));

        model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.None, 20), 
            StatModel.Init(0), 
            StatModel.Init(0));

        model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.Double, 20), 
            StatModel.Init(0), 
            StatModel.Init(0));

        state = model.AddPlayerStats(players[3].Id, 
            StatModel.Init(DartModifier.Triple, 20), 
            StatModel.Init(0), 
            StatModel.Init(0));

        //     p1  p2  p3  p4
        //      0  20  40  60
        //
        // 15   0   0   0   0
        // 16   0   0   0   0
        // 17   0   0   0   0
        // 18   0   0   0   0
        // 19   0   0   0   0
        // 20   x   Q   Q   Q
        // B    -   -   -   -

        var points = 0;
        foreach(var player in state.PlayerStates){
            Assert.True(player.Points == points);
            points += 20;
        }

        //fîfth round
        model.AddPlayerStats(players[0].Id, 
            StatModel.Init(DartModifier.Double, 25),
            StatModel.Init(25), 
            StatModel.Init(0));

        model.AddPlayerStats(players[1].Id, 
            StatModel.Init(DartModifier.Double, 25),
            StatModel.Init(25),  
            StatModel.Init(0));

        model.AddPlayerStats(players[2].Id, 
            StatModel.Init(DartModifier.Double, 25),
            StatModel.Init(25),  
            StatModel.Init(0));

        model.AddPlayerStats(players[3].Id, StatModel.Init(DartModifier.Double, 25));
        state = model.AddPlayerStats(players[3].Id, StatModel.Init(25));                         //finishing dart
        

        //     p1  p2  p3  p4
        //      0  20  40  60
        //
        // 15   0   0   0   0
        // 16   0   0   0   0
        // 17   0   0   0   0
        // 18   0   0   0   0
        // 19   0   0   0   0
        // 20   x   Q   Q   Q
        // B    0   0   0   0
        //
        //      4   3   2   1

        var rank = 4;
        foreach(var player in state.PlayerStates){
            Assert.True(player.Rank == rank);
            rank--;
        }
    }
}