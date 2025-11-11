using Flights.Domain.State;

namespace Flights.Infrastructure.Port;
public interface IStatRepository
{
    public Task<GameCountState> GetTotalGameCount(Guid tenantId);
    
    public Task<List<PlayerWins>> GetTotalPlayerWins(Guid tenantId);
}