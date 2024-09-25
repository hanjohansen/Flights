using Flights.Domain.Entities;

namespace Flights.Infrastructure.Port;
public interface IPlayerRepository
{
    public Task<PlayerEntity> CreatePlayer(string name);

    public Task<List<PlayerEntity>> GetPlayers();
}