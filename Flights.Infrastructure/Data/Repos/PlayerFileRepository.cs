using Flights.Domain.Entities;
using Flights.Domain.Exceptions;
using Flights.Infrastructure.Port;
using Microsoft.EntityFrameworkCore;

namespace Flights.Infrastructure.Data.Repos;

public class PlayerFileRepository : IPlayerFileRepository
{
    private readonly IDbContextFactory<FlightsDbContext> _dbFactory;

    public PlayerFileRepository(IDbContextFactory<FlightsDbContext> dbFactory){
        _dbFactory = dbFactory;
    }

    public async Task SetPlayerJingle(Guid playerId, string fileName, string storagePath){
        using var db = await _dbFactory.CreateDbContextAsync();

        var player = await db.Players
            .FirstOrDefaultAsync(p => p.Id == playerId && p.Deleted == false);

        if(player == null)
            throw new FlightsGameException("Player not found!");

        var playerFile = new PlayerFileEntity(){
            PlayerId = playerId,
            FileName = fileName,
            SourcePath = storagePath
        };

        await db.PlayerFiles.AddAsync(playerFile);
        await db.SaveChangesAsync();
    }

    public async Task DeletePlayerJingle(Guid fileId){
        using var db = await _dbFactory.CreateDbContextAsync();

        var file = await db.PlayerFiles
            .FirstOrDefaultAsync(x => x.Id == fileId);

        if(file == null)
            throw new FlightsGameException("File not found!");

        db.PlayerFiles.Remove(file);
        await db.SaveChangesAsync();
    }
}