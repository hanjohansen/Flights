using Flights.Domain.Entities.Game;
using Flights.Domain.Models;

namespace Flights.Test.Tournament;

public class TournamentAddPlayerTest
{
    [Fact]
    public void AddPlayer_Test()
    {
        var testHelpers = new TestHelpers();
        
        var players = testHelpers.GetPlayers(9);
        var lastPlayer = players.Last();
        
        players.Remove(lastPlayer);

        var model = TournamentModel.Create(Guid.Empty, players, 2, GameType.X01, false, 301, InOutModifier.None, InOutModifier.None);

        var gameId = model.Entity.Rounds.First().Games.Last().Id;
        
        model.AddPlayerToGame(gameId, lastPlayer);
        
        Assert.True(model.Entity.Rounds.Count == 1);
        Assert.True(model.Entity.Rounds.First().Games.Count == 4);
        Assert.False(model.Entity.Rounds.Last().Games.Last().Game!.Players.Count == 3);
    }
}