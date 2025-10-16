using Flights.Domain.State;

namespace Flights.Infrastructure.Port;
public interface IStatRepository
{
    public Task<GameCountState> GetTotalGameCount();
    
    public Task<List<PlayerWins>> GetTotalPlayerWins();
}