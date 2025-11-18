using Flights.Domain.Entities;
using Flights.Domain.Entities.Game;
using Flights.Domain.Exception;
using Flights.Domain.Models;

namespace Flights.Test.Base;
public class BaseGameModelTest
{
    [Fact]
    public void DartValue_Validation(){
        for(var i = 1; i <=20; i++){
            var dart = StatModel.Init(i);
            dart.Validate();

            dart = StatModel.Init(DartModifier.Double, i);
            dart.Validate();

            dart = StatModel.Init(DartModifier.Triple, i);
            dart.Validate();
        }

        var d = StatModel.Init(25);
        d.Validate();

        d = StatModel.Init(DartModifier.Double, 25);
        d.Validate();

        d = StatModel.Init(DartModifier.Triple, 25);
        Assert.Throws<FlightsGameException>(() => d.Validate());

        d = StatModel.Init(DartModifier.Triple, -1);
        Assert.Throws<FlightsGameException>(() => d.Validate());

        d = StatModel.Init(DartModifier.Triple, 22);
        Assert.Throws<FlightsGameException>(() => d.Validate());

        d = StatModel.Init(27);
        Assert.Throws<FlightsGameException>(() => d.Validate());
        
    }
    
    [Fact]
    public void GameRound_Should_Be_Added(){
        var players = GetTwoPlayers();

        var gameModel = GameModel.Create(Guid.Empty, players, GameType.X01, false,301, InOutModifier.None, InOutModifier.None);
        var gameState = gameModel.SolveGameState();

        Assert.True(gameState.Round == 1);

        gameModel.AddPlayerStats(
            players[0].Id,
            StatModel.Init(8),
            StatModel.Init(2),
            StatModel.Init(10));

        gameModel.AddPlayerStats(
            players[1].Id,
            StatModel.Init(8),
            StatModel.Init(2),
            StatModel.Init(10));

        gameModel.AddPlayerStats(
            players[0].Id,
            StatModel.Init(8),
            StatModel.Init(2),
            StatModel.Init(10));

        gameState = gameModel.SolveGameState();

        Assert.True(gameState.Round == 2);
    }

    [Fact]
    public void GameRound_Should_Not_AllowUnknownPlayer(){
        var players = GetTwoPlayers();

        var gameModel = GameModel.Create(Guid.Empty, players, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);
        gameModel.SolveGameState();

        Assert.Throws<FlightsGameException>(() => {
            gameModel.AddPlayerStats(
                Guid.NewGuid(), //!!!!
                StatModel.Init(8),
                StatModel.Init(2),
                StatModel.Init(10));
        });

        Assert.Throws<FlightsGameException>(() => {
            gameModel.AddPlayerStats(
                players[1].Id,  //!!!!
                StatModel.Init(8),
                StatModel.Init(2),
                StatModel.Init(10));
        });
    }
    
    private List<PlayerEntity> GetTwoPlayers(){
        return new List<PlayerEntity> { new(){Name = "Pierre", Id = Guid.NewGuid()}, new (){Name="Jens", Id = Guid.NewGuid()}};
    }
}