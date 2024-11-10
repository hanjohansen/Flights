using Flights.Domain.Entities;
using Flights.Domain.Exception;
using Flights.Domain.Models;
using Flights.Domain.State;
using Flights.Infrastructure.Port;
using Microsoft.EntityFrameworkCore;

namespace Flights.Infrastructure.Data.Repos;
public class GameRepository : IGameRepository
{
    private readonly IDbContextFactory<FlightsDbContext> _dbFactory;

    public GameRepository(IDbContextFactory<FlightsDbContext> dbFactory){
        _dbFactory = dbFactory;
    }
    public async Task<GameState> CreateGame(List<Guid> players, GameType type, int x01Target, InOutModifier inMod, InOutModifier outMod )
    {
        await using var db = await _dbFactory.CreateDbContextAsync();

        var allPlayers = await db.Players.ToListAsync();

        var selectedPlayers = players.Select(id => allPlayers.First(x => x.Id == id)).ToList();

        var model = GameModel.Create(selectedPlayers, type, x01Target, inMod, outMod);

        await db.Games.AddAsync(model.Entity);
        await db.SaveChangesAsync();

        var state = model.SolveGameState();
        return state;
    }

    public async Task<GameState> ReplayGame(Guid gameId)
    {
        var sourceGame = await GetGame((gameId));

        var players = sourceGame.Entity.Players.Select(x => x.Player.Id).ToList();
        
        return await CreateGame(players,
            sourceGame.Entity.Type,
            sourceGame.Entity.X01Target,
            sourceGame.Entity.InModifier,
            sourceGame.Entity.OutModifier);
    }

    public async Task<GameState> AddPlayerStat(Guid gameId, Guid playerId, StatModel stat)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();

        var game = await GetBaseQuery(db).FirstOrDefaultAsync(x => x.Id == gameId);

        if (game == null)
            throw new FlightsGameException("Game not found!");

        var model = GameModel.FromEntity(game);
        var newState = model.AddPlayerStats(playerId, stat);

        await db.SaveChangesAsync();
        return newState;
    }

    public async Task<List<GameEntity>> GetGames()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();

        var games = await GetBaseQuery(db)
            .AsNoTrackingWithIdentityResolution()
            .ToListAsync();

        return games.OrderByDescending(x => x.Started).ToList();
    }

    public async Task<GameModel> GetGame(Guid id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();

        var game = await GetBaseQuery(db)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (game == null)
            throw new FlightsGameException("Game not found!");

        return GameModel.FromEntity(game);
    }

    private IQueryable<GameEntity> GetBaseQuery(FlightsDbContext db)
    {
        var query = db.Games
            .AsSplitQuery()
            .Include(x => x.Players.OrderBy(y => y.OrderNumber))
            .ThenInclude(x => x.Player)
            .Include(x => x.Rounds.OrderBy(y => y.Number))
            .ThenInclude(x => x.RoundStats.OrderBy(y => y.OrderNumber));

        return query;
    }

    public async Task<GameState> RevertLastDart(Guid gameId)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();

        var game = await GetBaseQuery(db).FirstOrDefaultAsync(x => x.Id == gameId);

        if (game == null)
            throw new FlightsGameException("Game not found!");

        if (game.Finished != null)
            throw new FlightsGameException("Game is finished!");

        var model = GameModel.FromEntity(game);
        var newState = model.RevertLastDart();

        await db.SaveChangesAsync();
        return newState;
    }

    public async Task DeleteGame(Guid gameId)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        
        var game = await GetBaseQuery(db).FirstOrDefaultAsync(x => x.Id == gameId);

        if (game == null)
            throw new FlightsGameException("Game not found!");

        db.Games.Remove(game);
        await db.SaveChangesAsync();
    }
}