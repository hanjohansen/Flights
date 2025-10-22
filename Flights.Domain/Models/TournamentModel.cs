using Flights.Domain.Entities;
using Flights.Domain.Entities.Game;
using Flights.Domain.Entities.Tournament;
using Flights.Domain.Exception;
using Flights.Domain.State;
using Flights.Domain.State.Solvers;
using Flights.Util;

namespace Flights.Domain.Models;

public class TournamentModel
{
    public readonly TournamentEntity Entity;
    private readonly ITournamentSolver _solver;
    
    public static TournamentModel Create(List<PlayerEntity> players, int firstRoundPlayersPerGame, GameType type, bool semiFinalWithLosersCup, int x01Target, InOutModifier inMod, InOutModifier outMod)
    {
        if (players.Count < 4)
            throw new FlightsGameException("Minimum of four players required for tournament!");
        
        var tournament = new TournamentEntity
        {
            Type = type,
            SemiFinalWithLosersCup = semiFinalWithLosersCup,
            FirstRoundPlayersPerGame = firstRoundPlayersPerGame,
            X01Target = x01Target,
            InModifier = inMod,
            OutModifier = outMod,
        };
        
        players.ForEach(x => tournament.Players.Add(new TournamentPlayerEntity
        {
            OrderNumber = players.IndexOf(x) + 1,
            Player = x,
            PlayerId = x.Id,
            Tournament = tournament
        }));

        var model = new TournamentModel(tournament);
        model.ResolveTournamentState();

        return model;
    }

    public static TournamentModel FromEntity(TournamentEntity entity) => new(entity);

    private TournamentModel(TournamentEntity entity)
    {
        Entity = entity;
        _solver = GameSolverFactory.GetTournamentSolver(entity);
    }

    public TournamentState ResolveTournamentState()
    {
        return _solver.Solve();
    }

    public void SwitchPlayerOrder(Guid gameId)
    {
        var games = Entity.Rounds
            .SelectMany(x => x.Games.Where(y => y.Game != null))
            .Select(z => z.Game!)
            .ToList();

        var game = games.FirstOrDefault(x => x.Id == gameId);

        if (game == null)
            throw new FlightsGameException("Game not found");
        
        var started = game.Rounds
            .Any(y => y.RoundStats
                .Any(z => z.FirstDart != null || z.SecondDart != null || z.ThirdDart != null));
        if (started)
            throw new FlightsGameException("Game was already started!");

        game.Players.Reverse();
        game.Players.ForEach(x => { x.OrderNumber = game.Players.IndexOf(x) + 1; });
        
        game.Rounds.ForEach(x =>
        {
            x.RoundStats.Reverse();
            x.RoundStats.ForEach(y => { y.OrderNumber = x.RoundStats.IndexOf(y) + 1;});
        });
    }

    public void SkipLosersCup()
    {
        var regularGamesInLastRound = Entity.Rounds.Last().Games.Where(x => x.IsLosersCup == false).ToList();
        var losersCup = Entity.Rounds.Last().Games.FirstOrDefault(x => x.IsLosersCup);
        
        if (regularGamesInLastRound.Count != 2
            || losersCup == null) 
            throw new FlightsGameException("No Losers Cup to skip!");

        Entity.Rounds.Last().Games.Remove(losersCup);
        Entity.SemiFinalWithLosersCup = false;

        _solver.Solve();
    }

    public void DevFinishGame(Guid gameId)
    {
        var games = Entity.Rounds
            .SelectMany(x => x.Games.Where(y => y.Game != null))
            .Select(z => z.Game!)
            .ToList();

        var game = games.FirstOrDefault(x => x.Id == gameId);

        if (game == null)
            throw new FlightsGameException("Game not found");

        var lastRound = game.Rounds.Last();
        var random = new Random();
        
        foreach (var stat in lastRound.RoundStats)
        {
            stat.FirstDart ??= new DartStatEntity { Modifier = DartModifier.None, Value = 1 };
            
            stat.EndPoints = random.Next(2, 30);
            stat.Rank = lastRound.RoundStats.IndexOf(stat) + 1;
        }
        
        game.Finished = DateTimeOffset.UtcNow;

        _solver.Solve();
    }

    public void AddPlayerToGame(Guid tournamentGameId, PlayerEntity player)
    {
        var game = Entity.Rounds
            .SelectMany(x => x.Games)
            .FirstOrDefault(x => x.Id == tournamentGameId);

        if (game == null)
            throw new FlightsGameException("Game not found");
        
        if(game.Game == null)
            throw new FlightsGameException("Game is not ready yet");
            
        if(game.Game.Finished != null)
            throw new FlightsGameException("Game is already finished");
        
        var started = game.Game.Rounds
            .Any(y => y.RoundStats
                .Any(z => z.FirstDart != null || z.SecondDart != null || z.ThirdDart != null));
        
        if(started)
            throw new FlightsGameException("Game was already started");

        var isFinalOrSemi = game.TournamentRound.Games.Count == 1
                            || game.TournamentRound.Games.Count == 2;
        
        if(isFinalOrSemi)
            throw new FlightsGameException("Adding player in final- or semi-final round is not allowed");
        
        var existingPlayerIds = Entity.Players.Select(x => x.Player.Id).ToList();
        if(existingPlayerIds.Contains(player.Id))
            throw new FlightsGameException("Player is already playing");
        
        //add to tournament
        var playerOrderMax = Entity.Players.Max(x => x.OrderNumber);
        Entity.Players.Add(new TournamentPlayerEntity()
        {
            Player = player,
            PlayerId = player.Id,
            OrderNumber = playerOrderMax + 1
        });

        //add to game
        var gameOrderMax = game.Game.Players.Max(x => x.OrderNumber);
        game.Game.Players.Add(new GamePlayerEntity()
        {
            Player = player,
            PlayerId = player.Id,
            OrderNumber = gameOrderMax + 1
        });
        
        //add to first round
        var roundOrderMax = game.Game.Rounds.First().RoundStats.Max(x => x.OrderNumber);
        game.Game.Rounds.First().RoundStats.Add(new RoundStatEntity()
        {
            Player = player,
            PlayerId = player.Id,
            OrderNumber = roundOrderMax + 1
        });

    }

    public void ReshuffleLastRound()
    {
        var lastRound = Entity.Rounds.Last();
        
        if(lastRound.HasDarts())
            throw new FlightsGameException("Round was already started!");
        
        //approaches to reshuffling depend on the round number
        //a) its the first round or...
        //b) ...its any following round
        //
        //shuffling is automatically done for any new round except the first one.
        //the first round creates the player order according the provided players.
        
        //1. conditionally  reshuffle tournament players
        if (Entity.Rounds.Count == 1)
        {
            //shuffle + reindex
            Entity.Players.Shuffle();
            foreach (var player in Entity.Players)
                player.OrderNumber = Entity.Players.IndexOf(player) + 1;
        }
        
        //2. remove the last round for both cases
        Entity.Rounds.Remove(lastRound);
        
        //3. recreate shuffled round by resolving state
        ResolveTournamentState();
    }
}
