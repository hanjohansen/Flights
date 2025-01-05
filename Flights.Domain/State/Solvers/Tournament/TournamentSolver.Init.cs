using Flights.Domain.Entities;
using Flights.Domain.Entities.Tournament;
using Flights.Domain.Models;

namespace Flights.Domain.State.Solvers.Tournament;

public partial class TournamentSolver
{
    private void InitTournament()
    {
        var players = entity.Players.ToList();
        
        if(players.Count == 5)
            InitFivePlayerTournament(players);
        else  
            InitNewRound(players);
        
        entity.Started = DateTimeOffset.UtcNow;
    }

    private void InitFivePlayerTournament(List<TournamentPlayerEntity> tournamentPlayers)
    {
        var players = tournamentPlayers.ToList();
        //create round
        var round = new TournamentRoundEntity()
        {
            OrderNumber = entity.Rounds.Count + 1,
            Tournament = entity,
        };
        
        entity.Rounds.Add(round);
        
        //create games
        //first
        var gamePlayers = players.Take(2)
            .Select(x => x.Player)
            .ToList();

        var game = GameModel.Create(gamePlayers, entity.Type, true, entity.X01Target, entity.InModifier,
            entity.OutModifier);
            
        round.Games.Add(new TournamentGameEntity()
        {
            Game = game.Entity,
            TournamentRound = round,
            OrderNumber = 1
        });
        
        //second
        gamePlayers = players.TakeLast(3).Select(x => x.Player).ToList();
        game = GameModel.Create(gamePlayers, entity.Type, true, entity.X01Target, entity.InModifier,
            entity.OutModifier);
            
        round.Games.Add(new TournamentGameEntity()
        {
            Game = game.Entity,
            TournamentRound = round,
            OrderNumber = 2
        });
        
        //setup empty losers cup if necessary
        if (entity.SemiFinalWithLosersCup)
        {
            round.Games.Add(new TournamentGameEntity()
            {
                OrderNumber = 3,
                TournamentRound = round,
                IsLosersCup = true
            });
        }
    }

    private void InitNewRound(List<TournamentPlayerEntity> tournamentPlayers)
    {
        var isEven = tournamentPlayers.Count % 2 == 0;
        var players = tournamentPlayers.ToList();
        TournamentPlayerEntity? wildcard = null;

        if (!isEven)
        {
            wildcard = players.Last();
            players.Remove(wildcard);
        }

        //create round
        var round = new TournamentRoundEntity()
        {
            OrderNumber = entity.Rounds.Count + 1,
            Tournament = entity,
            WildCard = wildcard
        };
        
        entity.Rounds.Add(round);

        //set up games
        var gameOrderNumber = 1;
        for (int i = 0; i < players.Count; i += 2)
        {
            var gamePlayers = new List<PlayerEntity>()
            {
                players[i].Player,
                players[i + 1].Player
            };

            var game = GameModel.Create(gamePlayers, entity.Type, true, entity.X01Target, entity.InModifier,
                entity.OutModifier);
            
            round.Games.Add(new TournamentGameEntity()
            {
                Game = game.Entity,
                TournamentRound = round,
                OrderNumber = gameOrderNumber
            });

            gameOrderNumber++;
        }

        //setup empty losers cup if necessary
        var semiFinalWithLosersCup = round.Games.Count == 2 && entity.SemiFinalWithLosersCup;
        var nextRoundPlayers = round.Games.Count + (round.WildCard != null ? 1 : 0);
        if (nextRoundPlayers != 1 && (nextRoundPlayers % 2 != 0 || semiFinalWithLosersCup))
        {
            round.Games.Add(new TournamentGameEntity()
            {
                OrderNumber = gameOrderNumber,
                TournamentRound = round,
                IsLosersCup = true
            });
        }
    }

    private void InitLosersCup()
    {
        var lastRound = entity.Rounds.Last();
        var losers = new List<PlayerEntity>();
        var otherGames = lastRound.Games
            .Where(x => x.IsLosersCup == false)
            .ToList();

        foreach (var game in otherGames)
        {
            var gameLosers = game.Game!.Rounds
                .Last()
                .RoundStats
                .Where(x => x.Rank != 1)
                .ToList();

            foreach (var gameLoser in gameLosers)
                losers.Add(gameLoser.Player);
        }
        
        var loserCupGame = GameModel.Create(losers, entity.Type, true, entity.X01Target, entity.InModifier,
            entity.OutModifier);

        var loserCup = lastRound.Games.First(x => x.IsLosersCup);
        loserCup.Game = loserCupGame.Entity;
    }
}