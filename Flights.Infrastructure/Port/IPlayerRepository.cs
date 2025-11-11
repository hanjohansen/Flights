using Flights.Domain.Entities;

namespace Flights.Infrastructure.Port;
public interface IPlayerRepository
{
    public Task<PlayerEntity> CreatePlayer(Guid tenantId, string name);

    public Task<List<PlayerEntity>> GetPlayers(Guid tenantId);

    Task<PlayerEntity> GetPlayer(Guid playerId);

    Task<PlayerEntity> UpdatePlayer(Guid playerId, string newName);

    Task DeletePlayer(Guid playerId);
}