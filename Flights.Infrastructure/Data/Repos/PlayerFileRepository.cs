using Flights.Domain.Entities;
using Flights.Domain.Exception;
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
        await using var db = await _dbFactory.CreateDbContextAsync();

        var player = await db.Players
            .FirstOrDefaultAsync(p => p.Id == playerId && p.Deleted == false);

        if(player == null)
            throw new FlightsGameException("Player not found!");

        var playerFile = new PlayerFileEntity
        {
            PlayerId = playerId,
            FileType = PlayerFileType.Jingle,
            FileName = fileName,
            StoragePath = storagePath
        };

        await db.PlayerFiles.AddAsync(playerFile);
        await db.SaveChangesAsync();
    }

    public async Task TryDeletePlayerJingle(Guid playerId){
        await using var db = await _dbFactory.CreateDbContextAsync();

        var file = await db.PlayerFiles
            .FirstOrDefaultAsync(x => x.PlayerId == playerId && x.FileType == PlayerFileType.Jingle);

        if(file == null)
            return;

        db.PlayerFiles.Remove(file);
        await db.SaveChangesAsync();
    }

    public async Task DeletePlayerJingle(Guid fileId){
        await using var db = await _dbFactory.CreateDbContextAsync();

        var file = await db.PlayerFiles
            .FirstOrDefaultAsync(x => x.Id == fileId && x.FileType == PlayerFileType.Jingle);

        if(file == null)
            throw new FlightsGameException("File not found!");

        db.PlayerFiles.Remove(file);
        await db.SaveChangesAsync();
    }

    public async Task<PlayerFileEntity?> GetPlayerJingle(Guid playerId)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();

        var file = await db.PlayerFiles
            .AsNoTracking()
            .Include(x => x.Player)
            .FirstOrDefaultAsync(x => x.PlayerId == playerId && x.FileType == PlayerFileType.Jingle);

        return file;
    }

    public async Task<List<PlayerFileEntity>> GetPlayerJinglesByGame(Guid gameId)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();

        var game = await db.Games
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Players)
            .ThenInclude(x => x.Player)
            .ThenInclude(x => x.Files)
            .FirstOrDefaultAsync(x => x.Id == gameId);

        if(game == null)
            throw new FlightsGameException("Game not found");

        var files = game.Players.SelectMany(x => x.Player.Files.Where(y => y.FileType == PlayerFileType.Jingle)).ToList();

        return files;
    }
}