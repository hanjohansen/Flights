using Flights.Domain.Entities;
using Flights.Domain.Models;

namespace Flights.Test.X01;

public class X01GameTest{

    private TestHelpers _helper = new TestHelpers();

    [Fact]
    public void Initializes_Game_Correctly(){
        var players = _helper.GetPlayers(2);
        
        var model = GameModel.Create(
            players, 
            GameType.X01, 301, 
            InOutModifier.None, 
            InOutModifier.None);

        var state = model.SolveGameState(); 

        Assert.False(state.Finished);
        Assert.True(state.InModifier == InOutModifier.None);
        Assert.True(state.OutModifier == InOutModifier.None);

        foreach(var player in state.PlayerStates){
            Assert.False(player.IsIn);
            Assert.False(player.IsBust);
            Assert.True(player.Points == 301);
            Assert.Null(player.Rank);
        }
    }

    [Fact]
    public void SimpleIn(){
        var players = _helper.GetPlayers(2);
        
        var model = GameModel.Create(
            players, 
            GameType.X01, 301, 
            InOutModifier.None, 
            InOutModifier.None);

        //round 1
        model.AddPlayerStats(
            players[0].Id,
            StatModel.Init(5),
            StatModel.Init(10),
            StatModel.Init(8));  //23

        model.AddPlayerStats(
            players[1].Id,
            StatModel.Init(2),
            StatModel.Init(19),
            StatModel.Init(8));  //29

        //round 2
        model.AddPlayerStats(
            players[0].Id,
            StatModel.Init(1),
            StatModel.Init(3),
            StatModel.Init(17));  //21

        var state = model.AddPlayerStats(
            players[1].Id,
            StatModel.Init(8),
            StatModel.Init(12),
            StatModel.Init(5));  //25

        Assert.False(state.Finished);
        Assert.True(state.InModifier == InOutModifier.None);
        Assert.True(state.OutModifier == InOutModifier.None);

        foreach(var player in state.PlayerStates){
            Assert.True(player.IsIn);
            Assert.False(player.IsBust);
            Assert.Null(player.Rank);
        }

        Assert.True(state.PlayerStates[0].Points == 301 - 23 - 21); 
        Assert.True(state.PlayerStates[1].Points == 301 - 29 - 25); 
    }

    [Fact]
    public void DoubleIn(){
        var players = _helper.GetPlayers(2);
        
        var model = GameModel.Create(
            players, 
            GameType.X01, 301, 
            InOutModifier.Double, //!!!!! Double-in
            InOutModifier.None);

        //round 1
        model.AddPlayerStats(
            players[0].Id,
            StatModel.Init(5),
            StatModel.Init(10),
            StatModel.Init(8));  //no points

        model.AddPlayerStats(
            players[1].Id,
            StatModel.Init(2),
            StatModel.Init(DartModifier.Double, 4),
            StatModel.Init(8));  //16

        //round 2
        model.AddPlayerStats(
            players[0].Id,
            StatModel.Init(1),
            StatModel.Init(DartModifier.Triple, 3),
            StatModel.Init(17));  //no points

        var state = model.AddPlayerStats(
            players[1].Id,
            StatModel.Init(8),
            StatModel.Init(12),
            StatModel.Init(5));  //25

        Assert.False(state.Finished);
        Assert.True(state.InModifier == InOutModifier.Double);
        Assert.True(state.OutModifier == InOutModifier.None);

        Assert.True(state.PlayerStates[0].IsIn == false);
        Assert.True(state.PlayerStates[1].IsIn == true);

        foreach(var player in state.PlayerStates){
            Assert.False(player.IsBust);
            Assert.Null(player.Rank);
        }

        Assert.True(state.PlayerStates[0].Points == 301); 
        Assert.True(state.PlayerStates[1].Points == 301 - 16 - 25); 
    }   

    [Fact]
    public void TripleIn(){
        var players = _helper.GetPlayers(2);
        
        var model = GameModel.Create(
            players, 
            GameType.X01, 301, 
            InOutModifier.Triple, //!!!!! Triple-in
            InOutModifier.None);

        //round 1
        model.AddPlayerStats(
            players[0].Id,
            StatModel.Init(5),
            StatModel.Init(10),
            StatModel.Init(8));  //no points

        model.AddPlayerStats(
            players[1].Id,
            StatModel.Init(2),
            StatModel.Init(DartModifier.Double, 4),
            StatModel.Init(8));  // no points

        //round 2
        model.AddPlayerStats(
            players[0].Id,
            StatModel.Init(1),
            StatModel.Init(DartModifier.Triple, 3),
            StatModel.Init(17));  //26

        var state = model.AddPlayerStats(
            players[1].Id,
            StatModel.Init(8),
            StatModel.Init(12),
            StatModel.Init(5));  //no points

        Assert.False(state.Finished);
        Assert.True(state.InModifier == InOutModifier.Triple);
        Assert.True(state.OutModifier == InOutModifier.None);

        Assert.True(state.PlayerStates[0].IsIn == true);
        Assert.True(state.PlayerStates[1].IsIn == false);

        foreach(var player in state.PlayerStates){
            Assert.False(player.IsBust);
            Assert.Null(player.Rank);
        }

        Assert.True(state.PlayerStates[0].Points == 301 - 26); 
        Assert.True(state.PlayerStates[1].Points == 301); 
    }   

    [Fact]
    public void FullBullIn(){
        var players = _helper.GetPlayers(2);
        
        var model = GameModel.Create(
            players, 
            GameType.X01, 301, 
            InOutModifier.FullBull, //!!!!! FullBull-in
            InOutModifier.None);

        //round 1
        model.AddPlayerStats(
            players[0].Id,
            StatModel.Init(5),
            StatModel.Init(10),
            StatModel.Init(8));  //no points

        model.AddPlayerStats(
            players[1].Id,
            StatModel.Init(2),
            StatModel.Init(DartModifier.Double, 25),
            StatModel.Init(8));  // 58

        //round 2
        model.AddPlayerStats(
            players[0].Id,
            StatModel.Init(1),
            StatModel.Init(DartModifier.Triple, 3),
            StatModel.Init(17));  //no points

        var state = model.AddPlayerStats(
            players[1].Id,
            StatModel.Init(8),
            StatModel.Init(12),
            StatModel.Init(5));  //25

        Assert.False(state.Finished);
        Assert.True(state.InModifier == InOutModifier.FullBull);
        Assert.True(state.OutModifier == InOutModifier.None);

        Assert.True(state.PlayerStates[0].IsIn == false);
        Assert.True(state.PlayerStates[1].IsIn == true);

        foreach(var player in state.PlayerStates){
            Assert.False(player.IsBust);
            Assert.Null(player.Rank);
        }

        Assert.True(state.PlayerStates[0].Points == 301); 
        Assert.True(state.PlayerStates[1].Points == 301 - 58 - 25); 
    }   

    [Fact]
    public void SimpleOut(){
        var players = _helper.GetPlayers(3);
        
        var model = GameModel.Create(
            players, 
            GameType.X01, 20, 
            InOutModifier.None, 
            InOutModifier.None);

        //round 1
        model.AddPlayerStats(
            players[0].Id,
            StatModel.Init(1),
            StatModel.Init(10),
            StatModel.Init(2));  // -13

        model.AddPlayerStats(
            players[1].Id,
            StatModel.Init(1),
            StatModel.Init(1),
            StatModel.Init(1));  // -3

        model.AddPlayerStats(
            players[2].Id,
            StatModel.Init(1),
            StatModel.Init(3),
            StatModel.Init(1));  // -5

        //round 2
        model.AddPlayerStats(
            players[0].Id,
            StatModel.Init(7));  //winner!

        model.AddPlayerStats(
            players[1].Id,
            StatModel.Init(7));
        var state = model.AddPlayerStats(
            players[1].Id,
            StatModel.Init(10));  // 2nd winner!

        Assert.True(state.Finished);
        Assert.True(state.InModifier == InOutModifier.None);
        Assert.True(state.OutModifier == InOutModifier.None);

        Assert.True(state.PlayerStates[0].IsIn == true);
        Assert.True(state.PlayerStates[1].IsIn == true);
        Assert.True(state.PlayerStates[2].IsIn == true);

        foreach(var player in state.PlayerStates){
            Assert.False(player.IsBust);
            Assert.NotNull(player.Rank);
        }

        Assert.True(state.PlayerStates[0].Points == 0); 
        Assert.True(state.PlayerStates[1].Points == 0);
        Assert.True(state.PlayerStates[2].Points == 15);  

        Assert.True(state.PlayerStates[0].Rank == 1); 
        Assert.True(state.PlayerStates[1].Rank == 2);
        Assert.True(state.PlayerStates[2].Rank == 3);
    } 

    [Fact]
    public void DoubleOut(){
        var players = _helper.GetPlayers(3);
        
        var model = GameModel.Create(
            players, 
            GameType.X01, 20, 
            InOutModifier.None,
            InOutModifier.Double);  //!!!!! Double out

        //round 1
        model.AddPlayerStats(
            players[0].Id,
            StatModel.Init(2),
            StatModel.Init(2),
            StatModel.Init(2));  // -6

        model.AddPlayerStats(
            players[1].Id,
            StatModel.Init(2),
            StatModel.Init(1),
            StatModel.Init(1));  // -4

        model.AddPlayerStats(
            players[2].Id,
            StatModel.Init(2),
            StatModel.Init(3),
            StatModel.Init(1));  // -6

        //round 2
        model.AddPlayerStats(
            players[0].Id,
            StatModel.Init(20));  //bust

        model.AddPlayerStats(
            players[1].Id,
            StatModel.Init(20));  //bust

        model.AddPlayerStats(
            players[2].Id,
            StatModel.Init(DartModifier.Double, 7));  //winner!

        //round3
        var state = model.AddPlayerStats(
            players[0].Id,
            StatModel.Init(DartModifier.Double, 7));  //2nd winner

        Assert.True(state.Finished);
        Assert.True(state.InModifier == InOutModifier.None);
        Assert.True(state.OutModifier == InOutModifier.Double);

        Assert.True(state.PlayerStates[0].IsIn == true);
        Assert.True(state.PlayerStates[1].IsIn == true);
        Assert.True(state.PlayerStates[2].IsIn == true);

        foreach(var player in state.PlayerStates){
            Assert.False(player.IsBust);
            Assert.NotNull(player.Rank);
        }

        Assert.True(state.PlayerStates[0].Points == 0); 
        Assert.True(state.PlayerStates[1].Points == 16);
        Assert.True(state.PlayerStates[2].Points == 0);  

        Assert.True(state.PlayerStates[0].Rank == 2); 
        Assert.True(state.PlayerStates[1].Rank == 3);
        Assert.True(state.PlayerStates[2].Rank == 1);
    } 

    [Fact]
    public void TripleOut(){
        var players = _helper.GetPlayers(3);
        
        var model = GameModel.Create(
            players, 
            GameType.X01, 20, 
            InOutModifier.None, //!!!!! FullBull-in
            InOutModifier.Triple);

        //round 1
        model.AddPlayerStats(
            players[0].Id,
            StatModel.Init(4),
            StatModel.Init(2),
            StatModel.Init(2));  // -8

        model.AddPlayerStats(
            players[1].Id,
            StatModel.Init(2),
            StatModel.Init(2),
            StatModel.Init(1));  // -5

        model.AddPlayerStats(
            players[2].Id,
            StatModel.Init(2),
            StatModel.Init(5),
            StatModel.Init(1));  // -8

        //round 2
        model.AddPlayerStats(
            players[0].Id,
            StatModel.Init(20));  //bust

        model.AddPlayerStats(
            players[1].Id,
            StatModel.Init(DartModifier.Triple, 5));  //Winner

        var state = model.AddPlayerStats(
            players[2].Id,
            StatModel.Init(DartModifier.Triple, 4));  // 2nd winner!

        Assert.True(state.Finished);
        Assert.True(state.InModifier == InOutModifier.None);
        Assert.True(state.OutModifier == InOutModifier.Triple);

        Assert.True(state.PlayerStates[0].IsIn == true);
        Assert.True(state.PlayerStates[1].IsIn == true);
        Assert.True(state.PlayerStates[2].IsIn == true);

        foreach(var player in state.PlayerStates){
            if(state.PlayerStates.IndexOf(player) == 0){
                Assert.True(player.IsBust);
            }else{
                Assert.False(player.IsBust);
            }
            Assert.NotNull(player.Rank);
        }

        Assert.True(state.PlayerStates[0].Points == 12); 
        Assert.True(state.PlayerStates[1].Points == 0);
        Assert.True(state.PlayerStates[2].Points == 0);  

        Assert.True(state.PlayerStates[0].Rank == 3); 
        Assert.True(state.PlayerStates[1].Rank == 1);
        Assert.True(state.PlayerStates[2].Rank == 2);
    } 

    [Fact]
    public void FullBullOut(){
        var players = _helper.GetPlayers(3);
        
        var model = GameModel.Create(
            players, 
            GameType.X01, 60, 
            InOutModifier.None, //!!!!! FullBull-in
            InOutModifier.FullBull);

        //round 1
        model.AddPlayerStats(
            players[0].Id,
            StatModel.Init(6),
            StatModel.Init(2),
            StatModel.Init(2));  // -10

        model.AddPlayerStats(
            players[1].Id,
            StatModel.Init(8),
            StatModel.Init(1),
            StatModel.Init(1));  // -10

        model.AddPlayerStats(
            players[2].Id,
            StatModel.Init(6),
            StatModel.Init(3),
            StatModel.Init(1));  // -10

        //round 2
        model.AddPlayerStats(
            players[0].Id,
            StatModel.Init(DartModifier.Double,25));  // winner

        var state = model.AddPlayerStats(
            players[1].Id,
            StatModel.Init(DartModifier.Double,25));  // 2nd winner

        Assert.True(state.Finished);
        Assert.True(state.InModifier == InOutModifier.None);
        Assert.True(state.OutModifier == InOutModifier.FullBull);

        Assert.True(state.PlayerStates[0].IsIn == true);
        Assert.True(state.PlayerStates[1].IsIn == true);
        Assert.True(state.PlayerStates[2].IsIn == true);

        foreach(var player in state.PlayerStates){
            Assert.False(player.IsBust);
            Assert.NotNull(player.Rank);
        }

        Assert.True(state.PlayerStates[0].Points == 0); 
        Assert.True(state.PlayerStates[1].Points == 0);
        Assert.True(state.PlayerStates[2].Points == 50);  

        Assert.True(state.PlayerStates[0].Rank == 1); 
        Assert.True(state.PlayerStates[1].Rank == 2);
        Assert.True(state.PlayerStates[2].Rank == 3);
    } 
}