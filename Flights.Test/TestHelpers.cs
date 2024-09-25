using Flights.Domain.Entities;

namespace Flights.Test;
public class TestHelpers
{
    private List<PlayerEntity> _players = new();

    public TestHelpers(){
        SetupPlayers();
    }

    private void SetupPlayers(){
        _players.Add(new PlayerEntity(){Id = Guid.NewGuid(), Name="Hannes"});
        _players.Add(new PlayerEntity(){Id = Guid.NewGuid(), Name="Pierre"});
        _players.Add(new PlayerEntity(){Id = Guid.NewGuid(), Name="Arnim"});
        _players.Add(new PlayerEntity(){Id = Guid.NewGuid(), Name="Basti"});
        _players.Add(new PlayerEntity(){Id = Guid.NewGuid(), Name="Jens"});
    }

    public List<PlayerEntity> GetPlayers(int count){
        return _players.Take(count).ToList();
    }
}