using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flights.Domain.Entities;
using Flights.Domain.Models;

namespace Flights.Test.Shanghai
{
    public class ShanghaiGameTest
    {
        private TestHelpers _helper = new TestHelpers();

        [Fact]
        public void Initializes_Game_Correctly(){
            var players = _helper.GetPlayers(2);
            
            var model = GameModel.Create(
                players, 
                GameType.Shanghai, 0, 
                InOutModifier.None, 
                InOutModifier.None);

            var state = model.SolveGameState(); 

            Assert.False(state.Finished);
            Assert.True(state.InModifier == InOutModifier.None);
            Assert.True(state.OutModifier == InOutModifier.None);

            Assert.True(state.ShanghaiState!.CurrentTarget == 1);
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
                GameType.Shanghai, 
                0, 
                InOutModifier.None, 
                InOutModifier.None);

            var state =  model.SolveGameState(); 

            //first player 1-3

            Assert.True(state.ShanghaiState!.CurrentTarget == 1);

            state =  model.AddPlayerStats(players[0].Id, 
                StatModel.Init(DartModifier.None, 1));

            Assert.True(state.ShanghaiState!.CurrentTarget == 2);    

            state = model.AddPlayerStats(players[0].Id, 
                StatModel.Init(DartModifier.Double, 2));

            Assert.True(state.ShanghaiState!.CurrentTarget == 3);

            state = model.AddPlayerStats(players[0].Id, 
                StatModel.Init(DartModifier.Triple, 3));

            state = model.SolveGameState();

            var playerState = state.PlayerStates.First(x => x.PlayerId == players[0].Id);

            Assert.True(playerState.ShanghaiState!.V1 == 1);
            Assert.True(playerState.ShanghaiState!.V2 == 4);
            Assert.True(playerState.ShanghaiState!.V3 == 9);

            Assert.True(state.CurrentPlayerId == state.PlayerStates[1].PlayerId);
            Assert.True(state.ShanghaiState!.CurrentTarget == 1);

            //second player 1-3

            state =  model.AddPlayerStats(players[1].Id, 
                StatModel.Init(DartModifier.None, 0));

            Assert.True(state.ShanghaiState!.CurrentTarget == 2);    

            state = model.AddPlayerStats(players[1].Id, 
                StatModel.Init(DartModifier.Triple, 2));

            Assert.True(state.ShanghaiState!.CurrentTarget == 3);

            state = model.AddPlayerStats(players[1].Id, 
                StatModel.Init(DartModifier.None, 3));

            state = model.SolveGameState();

            playerState = state.PlayerStates.First(x => x.PlayerId == players[1].Id);

            Assert.True(playerState.ShanghaiState!.V1 == 0);
            Assert.True(playerState.ShanghaiState!.V2 == 6);
            Assert.True(playerState.ShanghaiState!.V3 == 3);

            Assert.True(state.CurrentPlayerId == state.PlayerStates[2].PlayerId);
            Assert.True(state.ShanghaiState!.CurrentTarget == 1);

            //third player 1-3

            state =  model.AddPlayerStats(players[2].Id, 
                StatModel.Init(DartModifier.None, 0));

            Assert.True(state.ShanghaiState!.CurrentTarget == 2);    

            state = model.AddPlayerStats(players[2].Id, 
                StatModel.Init(DartModifier.None, 0));

            Assert.True(state.ShanghaiState!.CurrentTarget == 3);

            state = model.AddPlayerStats(players[2].Id, 
                StatModel.Init(DartModifier.Triple, 3));

            state = model.SolveGameState();

            playerState = state.PlayerStates.First(x => x.PlayerId == players[2].Id);

            Assert.True(playerState.ShanghaiState!.V1 == 0);
            Assert.True(playerState.ShanghaiState!.V2 == 0);
            Assert.True(playerState.ShanghaiState!.V3 == 9);

            Assert.True(state.CurrentPlayerId == state.PlayerStates[0].PlayerId);
            Assert.True(state.ShanghaiState!.CurrentTarget == 4);


            // second round

            //first player 4-6

            state =  model.AddPlayerStats(players[0].Id, 
                StatModel.Init(DartModifier.None, 0));

            Assert.True(state.ShanghaiState!.CurrentTarget == 5);    

            state = model.AddPlayerStats(players[0].Id, 
                StatModel.Init(DartModifier.Double, 5));

            Assert.True(state.ShanghaiState!.CurrentTarget == 6);

            state = model.AddPlayerStats(players[0].Id, 
                StatModel.Init(DartModifier.None, 0));

            state = model.SolveGameState();

            playerState = state.PlayerStates.First(x => x.PlayerId == players[0].Id);

            Assert.True(playerState.ShanghaiState!.V4 == 0);
            Assert.True(playerState.ShanghaiState!.V5 == 10);
            Assert.True(playerState.ShanghaiState!.V6 == 0);

            Assert.True(state.CurrentPlayerId == state.PlayerStates[1].PlayerId);
            Assert.True(state.ShanghaiState!.CurrentTarget == 4);

            //second player 4-6

            state =  model.AddPlayerStats(players[1].Id, 
                StatModel.Init(DartModifier.None, 0));

            Assert.True(state.ShanghaiState!.CurrentTarget == 5);    

            state = model.AddPlayerStats(players[1].Id, 
                StatModel.Init(DartModifier.None, 0));

            Assert.True(state.ShanghaiState!.CurrentTarget == 6);

            state = model.AddPlayerStats(players[1].Id, 
                StatModel.Init(DartModifier.Triple, 6));

            state = model.SolveGameState();

            playerState = state.PlayerStates.First(x => x.PlayerId == players[1].Id);

            Assert.True(playerState.ShanghaiState!.V4 == 0);
            Assert.True(playerState.ShanghaiState!.V5 == 0);
            Assert.True(playerState.ShanghaiState!.V6 == 18);

            Assert.True(state.CurrentPlayerId == state.PlayerStates[2].PlayerId);
            Assert.True(state.ShanghaiState!.CurrentTarget == 4);

            //third player 4-6

            state =  model.AddPlayerStats(players[2].Id, 
                StatModel.Init(DartModifier.Double, 4));

            Assert.True(state.ShanghaiState!.CurrentTarget == 5);    

            state = model.AddPlayerStats(players[2].Id, 
                StatModel.Init(DartModifier.None, 0));

            Assert.True(state.ShanghaiState!.CurrentTarget == 6);

            state = model.AddPlayerStats(players[2].Id, 
                StatModel.Init(DartModifier.None, 0));

            state = model.SolveGameState();

            playerState = state.PlayerStates.First(x => x.PlayerId == players[2].Id);

            Assert.True(playerState.ShanghaiState!.V4 == 8);
            Assert.True(playerState.ShanghaiState!.V5 == 0);
            Assert.True(playerState.ShanghaiState!.V6 == 0);

            Assert.True(state.CurrentPlayerId == state.PlayerStates[0].PlayerId);
            Assert.True(state.ShanghaiState!.CurrentTarget == 7);
        }

    }
}