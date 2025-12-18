using Flights.Domain.Entities.Game;
using Flights.Domain.Exception;
using Flights.Domain.Models;
using Flights.Domain.ReadModels;
using Flights.Domain.State;
using Flights.Infrastructure.Port;
using Microsoft.EntityFrameworkCore;

namespace Flights.Infrastructure.Data.Repos;
public class GameRepository(IDbContextFactory<FlightsDbContext> dbFactory) : IGameRepository
{
    public async Task<GameState> CreateGame(Guid tenantId, List<Guid> players, GameType type, bool finishAfterFirstRank, int x01Target, InOutModifier inMod, InOutModifier outMod )
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var allPlayers = await db.Players.ToListAsync();

        var selectedPlayers = players.Select(id => allPlayers.First(x => x.Id == id)).ToList();

        var model = GameModel.Create(tenantId, selectedPlayers, type, finishAfterFirstRank, x01Target, inMod, outMod);
        model.Entity.GameNumber = await GetNextGameNumber(db);

        await db.Games.AddAsync(model.Entity);
        await db.SaveChangesAsync();

        var state = model.SolveGameState();
        return state;
    }

    public async Task<GameState> ReplayGame(Guid gameId)
    {
        var sourceGame = await GetGame((gameId));

        var players = sourceGame.Entity.Players.Select(x => x.Player.Id).ToList();
        
        return await CreateGame(
            sourceGame.Entity.TenantId,
            players,
            sourceGame.Entity.Type,
            sourceGame.Entity.FinishAfterFirstRank,
            sourceGame.Entity.X01Target,
            sourceGame.Entity.InModifier,
            sourceGame.Entity.OutModifier);
    }

    public async Task<GameState> AddPlayerStat(Guid gameId, Guid playerId, StatModel stat)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var game = await GetBaseQuery(db).Include(x => x.TournamentGame)
            .ThenInclude(x => x!.TournamentRound)
            .FirstOrDefaultAsync(x => x.Id == gameId);

        if (game == null)
            throw new FlightsGameException("Game not found!");

        var model = GameModel.FromEntity(game);
        var newState = model.AddPlayerStats(playerId, stat);

        await db.SaveChangesAsync();
        return newState;
    }

    public async Task<List<GameListItemReadModel>> GetGames(Guid tenantId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var result = new List<GameListItemReadModel>();
        
        //load games
        var games = await db.Games
            .AsSplitQuery()
            .AsNoTracking()
            .Include(x => x.Players)
            .ThenInclude(x => x.Player)
            .Where(x => x.TournamentGameId == null && x.TenantId == tenantId)
            .OrderByDescending(x => x.GameNumber)
            .Take(20)
            .ToListAsync();
        
        foreach (var gameEntity in games)
            result.Add(GameListItemReadModel.FromGame(gameEntity));
        
        //load tournaments
        var tournaments = await db.Tournaments
            .AsSplitQuery()
            .AsNoTracking()
            .Include(x => x.Players)
            .ThenInclude(x => x.Player)
            .OrderByDescending(x => x.TournamentNumber)
            .Where(x => x.TenantId == tenantId)
            .Take(20)
            .ToListAsync();
        
        foreach (var tournament in tournaments)
            result.Add(GameListItemReadModel.FromTournament(tournament));

        result = result.OrderByDescending(x => x.Started).Take(20).ToList();

        return result;
    }

    public async Task<GameModel> GetGame(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var game = await GetBaseQuery(db)
            .Include(x => x.TournamentGame)
            .ThenInclude(x => x!.TournamentRound)
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

    public async Task<(GameState, StatModel)> RevertLastDart(Guid gameId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var game = await GetBaseQuery(db)          
            .Include(x => x.TournamentGame)
            .ThenInclude(x => x!.TournamentRound)
            .FirstOrDefaultAsync(x => x.Id == gameId);

        if (game == null)
            throw new FlightsGameException("Game not found!");

        var model = GameModel.FromEntity(game);
        var newStateAndStat = model.RevertLastDart();

        await db.SaveChangesAsync();
        return newStateAndStat;
    }

    public async Task DeleteGame(Guid gameId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        
        var game = await GetBaseQuery(db).FirstOrDefaultAsync(x => x.Id == gameId);

        if (game == null)
            throw new FlightsGameException("Game not found!");

        db.Games.Remove(game);
        await db.SaveChangesAsync();
    }

    private async Task<int> GetNextGameNumber(FlightsDbContext db){
        try{
            var nr = await db.Games.Select(x => x.GameNumber).MaxAsync();
            return nr+=1;
        }
        catch(InvalidOperationException){
            return 1;
        }
    }

}