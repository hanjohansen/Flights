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

            // third round

            //first player 7-9

            state =  model.AddPlayerStats(players[0].Id, 
                StatModel.Init(DartModifier.Double, 7));  

            state = model.AddPlayerStats(players[0].Id, 
                StatModel.Init(DartModifier.None, 0));

            state = model.AddPlayerStats(players[0].Id, 
                StatModel.Init(DartModifier.None, 0));

            state = model.SolveGameState();

            playerState = state.PlayerStates.First(x => x.PlayerId == players[0].Id);

            Assert.True(playerState.ShanghaiState!.V7 == 14);
            Assert.True(playerState.ShanghaiState!.V8 == 0);
            Assert.True(playerState.ShanghaiState!.V9 == 0);

            Assert.True(state.CurrentPlayerId == state.PlayerStates[1].PlayerId);
            Assert.True(state.ShanghaiState!.CurrentTarget == 7);

            //second player 7-9

            state =  model.AddPlayerStats(players[1].Id, 
                StatModel.Init(DartModifier.None, 0)); 

            state = model.AddPlayerStats(players[1].Id, 
                StatModel.Init(DartModifier.None, 0));

            state = model.AddPlayerStats(players[1].Id, 
                StatModel.Init(DartModifier.Double, 9));

            state = model.SolveGameState();

            playerState = state.PlayerStates.First(x => x.PlayerId == players[1].Id);

            Assert.True(playerState.ShanghaiState!.V7 == 0);
            Assert.True(playerState.ShanghaiState!.V8 == 0);
            Assert.True(playerState.ShanghaiState!.V9 == 18);

            Assert.True(state.CurrentPlayerId == state.PlayerStates[2].PlayerId);
            Assert.True(state.ShanghaiState!.CurrentTarget == 7);

            //third player 7-9

            state =  model.AddPlayerStats(players[2].Id, 
                StatModel.Init(DartModifier.None, 0));   

            state = model.AddPlayerStats(players[2].Id, 
                StatModel.Init(DartModifier.Triple, 8));

            state = model.AddPlayerStats(players[2].Id, 
                StatModel.Init(DartModifier.None, 0));

            state = model.SolveGameState();

            playerState = state.PlayerStates.First(x => x.PlayerId == players[2].Id);

            Assert.True(playerState.ShanghaiState!.V7 == 0);
            Assert.True(playerState.ShanghaiState!.V8 == 24);
            Assert.True(playerState.ShanghaiState!.V9 == 0);

            Assert.True(state.CurrentPlayerId == state.PlayerStates[0].PlayerId);
            Assert.True(state.ShanghaiState!.CurrentTarget == 10);
        }

            [Fact]
            public void Game_AutoTest(){
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
}