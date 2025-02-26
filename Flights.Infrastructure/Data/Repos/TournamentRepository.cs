using Flights.Domain.Entities.Game;
using Flights.Domain.Entities.Tournament;
using Flights.Domain.Exception;
using Flights.Domain.Models;
using Flights.Domain.State;
using Flights.Infrastructure.Port;
using Microsoft.EntityFrameworkCore;

namespace Flights.Infrastructure.Data.Repos;

public class TournamentRepository(IDbContextFactory<FlightsDbContext> dbFactory) : ITournamentRepository
{
    public async Task<TournamentState> CreateTournament(List<Guid> players, int firstRoundPlayersPerGame, GameType type, bool semiFinalWithLosersCup, int x01Target, InOutModifier inMod, InOutModifier outMod)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        
        var allPlayers = await db.Players.ToListAsync();
        
        var selectedPlayers = players.Select(id => allPlayers.First(x => x.Id == id)).ToList();

        var model = TournamentModel.Create(selectedPlayers, firstRoundPlayersPerGame, type, semiFinalWithLosersCup, x01Target, inMod, outMod);
        model.Entity.TournamentNumber = await GetNextTournamentNumber(db);

        await db.Tournaments.AddAsync(model.Entity);
        await db.SaveChangesAsync();

        var state = model.ResolveTournamentState();
        return state;
    }

    public async Task<TournamentModel> GetTournament(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        
        var tournament = await GetBaseQuery(db)
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if (tournament == null)
            throw new FlightsGameException("Tournament not found!");

        var model = TournamentModel.FromEntity(tournament);
        model.ResolveTournamentState();

        await db.SaveChangesAsync();

        return model;
    }

    public async Task<TournamentState> ReplayTournament(Guid id)
    {
        var t = await GetTournament(id);

        var players = t.Entity.Players.Select(x => x.Player.Id).ToList();

        return await CreateTournament(
            players,
            t.Entity.FirstRoundPlayersPerGame,
            t.Entity.Type,
            t.Entity.SemiFinalWithLosersCup,
            t.Entity.X01Target,
            t.Entity.InModifier,
            t.Entity.OutModifier);
    }

    public async Task DeleteTournament(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var tournament = await GetBaseQuery(db).FirstOrDefaultAsync(x => x.Id == id);
        
        if (tournament == null)
            throw new FlightsGameException("Tournament not found!");

        db.Tournaments.Remove(tournament);
        await db.SaveChangesAsync();
    }

    public async Task<TournamentState> SwitchPlayerOrder(Guid tournamentId, Guid gameId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var tournament = await GetBaseQuery(db).FirstOrDefaultAsync(x => x.Id == tournamentId);
        
        if (tournament == null)
            throw new FlightsGameException("Tournament not found!");

        var model = TournamentModel.FromEntity(tournament);
        model.SwitchPlayerOrder(gameId);

        await db.SaveChangesAsync();

        return model.ResolveTournamentState();
    }

    public async Task<TournamentState> SkipLosersCup(Guid tournamentId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var tournament = await GetBaseQuery(db).FirstOrDefaultAsync(x => x.Id == tournamentId);
        
        if (tournament == null)
            throw new FlightsGameException("Tournament not found!");

        var model = TournamentModel.FromEntity(tournament);
        model.SkipLosersCup();
        
        await db.SaveChangesAsync();

        return model.ResolveTournamentState();
    }

    public async Task<TournamentState> DevFinishGame(Guid tournamentId, Guid gameId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var tournament = await GetBaseQuery(db).FirstOrDefaultAsync(x => x.Id == tournamentId);
        
        if (tournament == null)
            throw new FlightsGameException("Tournament not found!");

        var model = TournamentModel.FromEntity(tournament);
        model.DevFinishGame(gameId);

        await db.SaveChangesAsync();

        return model.ResolveTournamentState();
    }

    public async Task<TournamentState> AddPlayerToGame(Guid tournamentId, Guid tournamentGameId, Guid playerId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var tournament = await GetBaseQuery(db).FirstOrDefaultAsync(x => x.Id == tournamentId);
        
        if (tournament == null)
            throw new FlightsGameException("Tournament not found!");
        
        var player = await db.Players.FirstOrDefaultAsync(x => x.Id == playerId);
        
        if(player == null)
            throw new FlightsGameException("Player not found!");
        
        var model = TournamentModel.FromEntity(tournament);
        model.AddPlayerToGame(tournamentGameId, player);
        
        await db.SaveChangesAsync();
        
        return model.ResolveTournamentState();
    }

    private IQueryable<TournamentEntity> GetBaseQuery(FlightsDbContext db)
    {
        var query = db.Tournaments
            .AsSplitQuery()
            .Include(x => x.Players.OrderBy(y => y.OrderNumber))
            .ThenInclude(x => x.Player)

            .Include(x => x.Rounds.OrderBy(y => y.OrderNumber))
            .ThenInclude(x => x.Games.OrderBy(y => y.OrderNumber))
            .ThenInclude(x => x.Game)
            .ThenInclude(x => x!.Players)

            .Include(x => x.Rounds.OrderBy(y => y.OrderNumber))
            .ThenInclude(x => x.Games.OrderBy(y => y.OrderNumber))
            .ThenInclude(x => x.Game)
            .ThenInclude(x => x!.Rounds.OrderBy(y => y.Number))
            .ThenInclude(x => x.RoundStats.OrderBy(y => y.OrderNumber))
            .ThenInclude(x => x.Player);

        return query;
    }

    private async Task<int> GetNextTournamentNumber(FlightsDbContext db){
        try{
            var nr = await db.Tournaments.Select(x => x.TournamentNumber).MaxAsync();
            return nr += 1;
        }catch(InvalidOperationException _){
            return 1;
        }
    }

}