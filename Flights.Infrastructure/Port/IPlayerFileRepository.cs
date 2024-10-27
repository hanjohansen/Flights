namespace Flights.Infrastructure.Port;

public interface IPlayerFileRepository
{
    Task SetPlayerJingle(Guid playerId, string fileName, string storagePath);

    Task DeletePlayerJingle(Guid fileId);
}