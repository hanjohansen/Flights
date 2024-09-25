using Flights.Domain.Entities;
using Flights.Domain.Exceptions;
using Flights.Infrastructure.Port;
using Microsoft.EntityFrameworkCore;

namespace Flights.Infrastructure.Data.Repos;
public class PlayerRepository : IPlayerRepository
{
    private readonly IDbContextFactory<FlightsDbContext> _dbFactory;

    public PlayerRepository(IDbContextFactory<FlightsDbContext> dbFactory){
        _dbFactory = dbFactory;
    }

    public async Task<PlayerEntity> CreatePlayer(string name)
    {
        using var db = await _dbFactory.CreateDbContextAsync();

        if(string.IsNullOrEmpty(name))
            throw new FlightsGameException("Name must not be empty!");

        var players = await GetPlayersInternal(db);

        if(players.Any(x => x.Name == name))
            throw new FlightsGameException("A player with that name already exists!");

        var player = new PlayerEntity(){Name = name};

        await db.Players.AddAsync(player);
        await db.SaveChangesAsync();

        return player;
    }

    public async Task<List<PlayerEntity>> GetPlayers()
    {
        using var db = await _dbFactory.CreateDbContextAsync();

        return await GetPlayersInternal(db);
    }

    private async Task<List<PlayerEntity>> GetPlayersInternal(FlightsDbContext db){
        var players = await db.Players
        .AsNoTracking()
        .OrderBy(x => x.Name)
        .ToListAsync();

        return players;
    }
}