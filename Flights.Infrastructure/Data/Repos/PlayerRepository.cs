using Flights.Domain.Entities;
using Flights.Domain.Exception;
using Flights.Infrastructure.Port;
using Microsoft.EntityFrameworkCore;

namespace Flights.Infrastructure.Data.Repos;
public class PlayerRepository(IDbContextFactory<FlightsDbContext> dbFactory) : IPlayerRepository
{
    public async Task<PlayerEntity> CreatePlayer(Guid tenantId, string name)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        if(string.IsNullOrEmpty(name))
            throw new FlightsGameException("Name must not be empty!");

        var players = await GetPlayersReadOnlyInternal(db, tenantId, true);

        if(players.Any(x => x.Name == name))
            throw new FlightsGameException("A player with that name already exists!");

        var player = new PlayerEntity {TenantId = tenantId, Name = name};

        await db.Players.AddAsync(player);
        await db.SaveChangesAsync();

        return player;
    }

    public async Task<PlayerEntity> GetPlayer(Guid playerId){
        await using var db = await dbFactory.CreateDbContextAsync();

        var player = await GetPlayerInternal(db, playerId, readOnly:true);

        if(player == null)
            throw new FlightsGameException("Player not found!");

        return player;
    }

    public async Task<PlayerEntity> UpdatePlayer(Guid playerId, string newName)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var player = await GetPlayerInternal(db, playerId, readOnly:false);
        var allPlayers = await GetPlayersReadOnlyInternal(db, player.TenantId, true);

        var existing = allPlayers.FirstOrDefault(x => x.Name == newName && x.Id != playerId);

        if(existing != null)
            throw new FlightsGameException("A player with that name already exists!");

        player.Name = newName;
        await db.SaveChangesAsync();

        return player;
    }

    public async Task<List<PlayerEntity>> GetPlayers(Guid tenantId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        return await GetPlayersReadOnlyInternal(db, tenantId, false);
    }

    private async Task<PlayerEntity> GetPlayerInternal(FlightsDbContext db, Guid playerId, bool readOnly){
        var query = db.Players.AsQueryable();

        if(readOnly)
            query = query.AsNoTracking();

        var player = await query.FirstOrDefaultAsync(x => x.Id == playerId);

        if(player == null)
            throw new FlightsGameException("Player not found!");

        return player;
    }

    private async Task<List<PlayerEntity>> GetPlayersReadOnlyInternal(FlightsDbContext db, Guid tenantId, bool includeDeleted){
        var query = db.Players
            .AsNoTracking();

        if (!includeDeleted)
            query = query.Where(x => x.TenantId == tenantId && x.Deleted == false);
        else
            query = query.Where(x => x.TenantId == tenantId);
        
        var players = await query
            .OrderBy(x => x.Name)
            .ToListAsync();

        return players;
    }

    public async Task DeletePlayer(Guid playerId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var player = await GetPlayerInternal(db, playerId, readOnly: false);

        player.Deleted = true;
        await db.SaveChangesAsync();
    }
}