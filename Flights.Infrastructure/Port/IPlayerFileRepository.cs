using Flights.Domain.Entities;

namespace Flights.Infrastructure.Port;

public interface IPlayerFileRepository
{
    Task<PlayerFileEntity?> GetPlayerJingle(Guid playerId);

    Task SetPlayerJingle(Guid playerId, string fileName, string storagePath);

    Task TryDeletePlayerJingle(Guid playerId);

    Task DeletePlayerJingle(Guid fileId);
}