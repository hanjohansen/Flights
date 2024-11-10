using Flights.Domain.Entities;
using Flights.Domain.Models;
using Flights.Domain.State;

namespace Flights.Infrastructure.Port;

public interface IGameRepository
{
    Task<GameState> CreateGame(List<Guid> players, GameType type, int x01Target, InOutModifier inMod,
        InOutModifier outMod);

    Task<GameState> ReplayGame(Guid gameId);

    Task<GameState> AddPlayerStat(Guid gameId, Guid playerId, StatModel stat);

    Task<GameState> RevertLastDart(Guid gameId);

    Task<List<GameEntity>> GetGames();

    Task<GameModel> GetGame(Guid id);

    Task DeleteGame(Guid gameId);
}