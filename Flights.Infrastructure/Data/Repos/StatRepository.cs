using Flights.Domain.Entities;
using Flights.Domain.State;
using Flights.Infrastructure.Port;
using Microsoft.EntityFrameworkCore;

namespace Flights.Infrastructure.Data.Repos;

public class StatRepository : IStatRepository
{
    private readonly IDbContextFactory<FlightsDbContext> _dbFactory;

    public StatRepository(IDbContextFactory<FlightsDbContext> dbFactory){
        _dbFactory = dbFactory;
    }

    public async Task<GameCountState> GetTotalGameCount(){
        await using var db = await _dbFactory.CreateDbContextAsync();

        var games = await db.Games
            .AsNoTracking()
            .Include(x => x.Players)
            .ThenInclude(x => x.Player)
            .ToListAsync();

        int? x01 = null;
        int? cricket = null;
        int? ctCricket = null;
        int? shanghai = null;
        var playerGames = new Dictionary<string, GameCountState>();

        foreach(var game in games){
            //count games
            switch(game.Type){
                case GameType.X01:
                    x01 = x01 == null ? 1 : x01 + 1;
                    break;
                case GameType.Cricket:
                    cricket = cricket == null ? 1 : cricket + 1;
                    break;
                case GameType.CtCricket:
                    ctCricket = ctCricket == null ? 1 : ctCricket +1; 
                    break;
                case GameType.Shanghai:
                    shanghai = shanghai == null ? 1 : shanghai + 1;
                    break;
            }

            // count per player
            foreach(var player in game.Players){
                if(player.Player.Deleted)
                    continue;
                    
                var playerName = player.Player.Name;

                if(!playerGames.ContainsKey(playerName))
                    playerGames.Add(playerName, new GameCountState(0,0,0,0,0, new()));

                switch(game.Type){
                    case GameType.X01:
                        playerGames[playerName] = playerGames[playerName] with {X01 = playerGames[playerName].X01 + 1};
                        break;
                    case GameType.Cricket:
                        playerGames[playerName] = playerGames[playerName] with {Cricket = playerGames[playerName].Cricket + 1};
                        break;
                    case GameType.CtCricket:
                        playerGames[playerName] = playerGames[playerName] with {CtCricket = playerGames[playerName].CtCricket + 1};
                        break;
                    case GameType.Shanghai:
                        playerGames[playerName] = playerGames[playerName] with {Shanghai = playerGames[playerName].Shanghai + 1};
                        break;
                }
            }
        }

        var playerStats = new List<PlayerGameCount>();

        foreach(var player in playerGames.Keys){
            var count = playerGames[player];
            var newCount = count with {Total = count.X01 + count.Cricket + count.CtCricket + count.Shanghai};
            playerStats.Add(new PlayerGameCount(player, newCount));
        }

        return new GameCountState(games.Count, x01, cricket, ctCricket, shanghai, playerStats.OrderByDescending(x => x.GameCount.Total).ToList());
    }
}
