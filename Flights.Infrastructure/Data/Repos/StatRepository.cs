using Flights.Domain.Entities.Game;
using Flights.Domain.State;
using Flights.Infrastructure.Port;
using Microsoft.EntityFrameworkCore;

namespace Flights.Infrastructure.Data.Repos;

public class StatRepository(IDbContextFactory<FlightsDbContext> dbFactory) : IStatRepository
{
    public async Task<GameCountState> GetTotalGameCount(Guid tenantId){
        await using var db = await dbFactory.CreateDbContextAsync();

        var games = await db.Games
            .AsNoTracking()
            .Include(x => x.Players)
            .ThenInclude(x => x.Player)
            .Where(x  => x.TenantId == tenantId)
            .ToListAsync();

        int? x01 = null;
        int? cricket = null;
        int? ctCricket = null;
        int? atClock = null;
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
                case GameType.AroundTheClock:
                    atClock = atClock == null ? 1 : atClock + 1;
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
                    case GameType.AroundTheClock:
                        playerGames[playerName] = playerGames[playerName] with {AroundTheClock = playerGames[playerName].AroundTheClock + 1};
                        break;
                }
            }
        }

        var playerStats = new List<PlayerGameCount>();

        foreach(var player in playerGames.Keys){
            var count = playerGames[player];
            var newCount = count with {Total = count.X01 + count.Cricket + count.CtCricket + count.AroundTheClock};
            playerStats.Add(new PlayerGameCount(player, newCount));
        }

        return new GameCountState(games.Count, x01, cricket, ctCricket, atClock, playerStats.OrderByDescending(x => x.GameCount.Total).ToList());
    }

    public async Task<List<PlayerWins>> GetTotalPlayerWins(Guid tenantId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var lastRounds = await db.Games
            .AsNoTrackingWithIdentityResolution()
            .Include(x => x.Rounds)
            .ThenInclude(x => x.RoundStats)
            .ThenInclude(x => x.Player)
            .Include(x => x.Rounds)
            .ThenInclude(x => x.Game)
            .Select(x => x.Rounds.OrderBy(y => y.Number).LastOrDefault())
            .Where(x => x!.Game.TenantId == tenantId)
            .ToListAsync();

        var playerDict = new Dictionary<Guid, PlayerWins>();

        foreach (var round in lastRounds)
            if(round != null)
                foreach(var stat in round.RoundStats)
                {
                    var player = stat.Player;
                    var playerId = player.Id;
                    
                    if(!playerDict.ContainsKey(playerId))
                        playerDict.Add(playerId, new PlayerWins(player.Name));
                    
                    var playerWins = playerDict[playerId];
                    var gameWins = playerWins.X01;
                    
                    //get wins by type
                    switch (round.Game.Type)
                    {
                        case GameType.X01:
                            break;
                        case GameType.Cricket:
                            gameWins = playerWins.Cricket;
                            break;
                        case GameType.CtCricket:
                            gameWins = playerWins.CtCricket;
                            break;
                        case  GameType.AroundTheClock:
                            gameWins = playerWins.AroundTheClock;
                            break;
                    }
                    
                    //update game count
                    gameWins.Games++;

                    //update wins
                    switch (stat.Rank)
                    {
                        case 1:
                            gameWins.First++;
                            break;
                        case 2:
                            gameWins.Second++;
                            break;
                        case 3:
                            gameWins.Third++;
                            break;
                    }
                }

        var players = playerDict.Values.ToList();
        
        foreach (var player in players)
        {
            var fP = 1m;
            var sP = .5m;
            var tP = .1m;
            
            //totals
            player.X01.Points = player.X01.First * fP + player.X01.Second * sP +  player.X01.Third * tP;
            player.Cricket.Points  = player.Cricket.First * fP + player.Cricket.Second * sP +  player.Cricket.Third * tP;
            player.CtCricket.Points  = player.CtCricket.First * fP + player.CtCricket.Second * sP +  player.CtCricket.Third * tP;
            player.AroundTheClock.Points  = player.AroundTheClock.First * fP + player.AroundTheClock.Second * sP +  player.AroundTheClock.Third * tP;

            //game ratios
            player.X01.PointsToGamesRatio = Math.Round(player.X01.Games != 0 ? player.X01.Points * 1m / player.X01.Games : 0, 4);
            player.X01.FirstToGamesRatio = Math.Round(player.X01.Games != 0 ? player.X01.First * 1m/ player.X01.Games : 0, 4);
            
            player.Cricket.PointsToGamesRatio = Math.Round(player.Cricket.Games != 0 ? player.Cricket.Points * 1m / player.Cricket.Games : 0, 4);
            player.Cricket.FirstToGamesRatio = Math.Round(player.Cricket.Games != 0 ? player.Cricket.First * 1m / player.Cricket.Games : 0, 4);
            
            player.CtCricket.PointsToGamesRatio = Math.Round(player.CtCricket.Games != 0 ? player.CtCricket.Points * 1m / player.CtCricket.Games : 0, 4);
            player.CtCricket.FirstToGamesRatio = Math.Round(player.CtCricket.Games != 0 ? player.CtCricket.First * 1m / player.CtCricket.Games : 0, 4);
            
            player.AroundTheClock.PointsToGamesRatio = Math.Round(player.AroundTheClock.Games != 0 ? player.AroundTheClock.Points * 1m / player.AroundTheClock.Games : 0, 4);
            player.AroundTheClock.FirstToGamesRatio = Math.Round(player.AroundTheClock.Games != 0 ? player.AroundTheClock.First * 1m / player.AroundTheClock.Games : 0, 4);
            
            //grand total
            var totalFirst = player.X01.First + player.Cricket.First + player.CtCricket.First + player.AroundTheClock.First;
            var totalSecond = player.X01.Second  + player.Cricket.Second + player.CtCricket.Second + player.AroundTheClock.Second;
            var totalThird = player.X01.Third  + player.Cricket.Third + player.CtCricket.Third + player.AroundTheClock.Third;
            var totalGames = player.X01.Games + player.Cricket.Games + player.CtCricket.Games + player.AroundTheClock.Games;
            var totalPoints = player.X01.Points + player.Cricket.Points + player.CtCricket.Points + player.AroundTheClock.Points; 
            
            var pointRatio = Math.Round(totalGames != 0 ? totalPoints * 1m / totalGames : 0, 4);
            var firstRatio = Math.Round(totalGames != 0 ? totalFirst * 1m / totalGames : 0, 4);
            
            player.Total = new ()
            {
                First = totalFirst,
                Second = totalSecond,
                Third = totalThird,
                Games = totalGames,
                Points = totalPoints,
                PointsToGamesRatio = pointRatio,
                FirstToGamesRatio = firstRatio
            };
        }

        return players.OrderByDescending(x => x.Total.PointsToGamesRatio).ToList();
    }
}
