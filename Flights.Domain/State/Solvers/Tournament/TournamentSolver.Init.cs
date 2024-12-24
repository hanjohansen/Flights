using Flights.Domain.Entities;
using Flights.Domain.Entities.Tournament;
using Flights.Domain.Models;

namespace Flights.Domain.State.Solvers.Tournament;

public partial class TournamentSolver
{
    private void InitTournament()
    {
        var players = entity.Players.ToList();
        InitNewRound(players);
        
        entity.Started = DateTimeOffset.UtcNow;
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