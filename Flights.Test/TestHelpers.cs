using Flights.Domain.Entities;
using Flights.Domain.Entities.Game;
using Flights.Domain.Exception;

namespace Flights.Test;
public class TestHelpers
{
    private readonly List<PlayerEntity> _players = new();

    private readonly Random _random = new(); 

    public TestHelpers(){
        SetupPlayers();
    }

    private void SetupPlayers(){
        _players.Add(new PlayerEntity {Id = Guid.NewGuid(), Name="Hannes"});
        _players.Add(new PlayerEntity {Id = Guid.NewGuid(), Name="Pierre"});
        _players.Add(new PlayerEntity {Id = Guid.NewGuid(), Name="Arnim"});
        _players.Add(new PlayerEntity {Id = Guid.NewGuid(), Name="Basti"});
        _players.Add(new PlayerEntity {Id = Guid.NewGuid(), Name="Jens"});
        _players.Add(new PlayerEntity {Id = Guid.NewGuid(), Name="Jens K."});
        _players.Add(new PlayerEntity {Id = Guid.NewGuid(), Name="Martin"});
    }

    public List<PlayerEntity> GetPlayers(int count){
        return _players.Take(count).ToList();
    }

    public int GetRandom(List<int> from){
        var randIndex = _random.Next(0, from.Count);

        return from[randIndex];
    }

    public void FinishGame(GameEntity game)
    {
        if (game.Finished != null)
            throw new FlightsGameException("game is already finished");
        
        game.Finished = DateTimeOffset.UtcNow;
        var lastRound = game.Rounds.Last();

        foreach (var player in lastRound.RoundStats)
            player.Rank = lastRound.RoundStats.IndexOf(player) + 1;
    }
}