using Flights.Domain.Entities;
using Flights.Domain.Exceptions;
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
        using var db = await _dbFactory.CreateDbContextAsync();

        var allPlayers = await db.Players.ToListAsync();

        var selectedPlayers = new List<PlayerEntity>();

        foreach (var id in players)
        {
            var player = allPlayers.First(x => x.Id == id);
            selectedPlayers.Add(player);
        }
        
        var model = GameModel.Create(selectedPlayers, type, x01Target, inMod, outMod);

        await db.Games.AddAsync(model.Entity);
        await db.SaveChangesAsync();

        var state = model.SolveGameState();
        return state;
    }

    public async Task<GameState> AddPlayerStat(Guid gameId, Guid playerId, StatModel stat)
    {
        using var db = await _dbFactory.CreateDbContextAsync();

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
        using var db = await _dbFactory.CreateDbContextAsync();

        var games = await GetBaseQuery(db)
            .AsNoTrackingWithIdentityResolution()
            .ToListAsync();

        return games.OrderByDescending(x => x.Started).ToList();
    }

    public async Task<GameModel> GetGame(Guid id)
    {
        using var db = await _dbFactory.CreateDbContextAsync();

        var game = await GetBaseQuery(db)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (game == null)
            throw new FlightsGameException("Game not found!");

        return GameModel.FromEntity(game);
    }

    public IQueryable<GameEntity> GetBaseQuery(FlightsDbContext db)
    {
        var query = db.Games
            .AsSplitQuery()
            .Include(x => x.Players.OrderBy(x => x.OrderNumber))
            .ThenInclude(x => x.Player)
            .Include(x => x.Rounds.OrderBy(x => x.Number))
            .ThenInclude(x => x.RoundStats.OrderBy(x => x.OrderNumber));

        return query;
    }

    public async Task<GameState> RevertLastDart(Guid gameId)
    {
        using var db = await _dbFactory.CreateDbContextAsync();

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

    public async Task<GameState> FinalizeGame(Guid gameId)
    {
        using var db = await _dbFactory.CreateDbContextAsync();

        var game = await GetBaseQuery(db).FirstOrDefaultAsync(x => x.Id == gameId);

        if (game == null)
            throw new FlightsGameException("Game not found!");

        if (game.Finished != null)
            throw new FlightsGameException("Game is finished!");

        var model = GameModel.FromEntity(game);
        var newState = model.FinalizeGame();

        await db.SaveChangesAsync();
        return newState;
    }
}