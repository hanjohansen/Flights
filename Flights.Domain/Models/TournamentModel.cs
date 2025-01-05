using Flights.Domain.Entities;
using Flights.Domain.Entities.Game;
using Flights.Domain.Entities.Tournament;
using Flights.Domain.Exception;
using Flights.Domain.State;
using Flights.Domain.State.Solvers;

namespace Flights.Domain.Models;

public class TournamentModel
{
    public readonly TournamentEntity Entity;
    private readonly ITournamentSolver _solver;
    
    public static TournamentModel Create(List<PlayerEntity> players, GameType type, bool semiFinalWithLosersCup, int x01Target, InOutModifier inMod, InOutModifier outMod)
    {
        if (players.Count < 4)
            throw new FlightsGameException("Minimum of four players required for tournament!");
        
        var tournament = new TournamentEntity()
        {
            Type = type,
            SemiFinalWithLosersCup = semiFinalWithLosersCup,
            X01Target = x01Target,
            InModifier = inMod,
            OutModifier = outMod,
        };
        
        players.ForEach(x => tournament.Players.Add(new TournamentPlayerEntity()
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
            stat.FirstDart ??= new DartStatEntity() { Modifier = DartModifier.None, Value = 1 };
            
            stat.EndPoints = random.Next(2, 30);
            stat.Rank = lastRound.RoundStats.IndexOf(stat) + 1;
        }
        
        game.Finished = DateTimeOffset.UtcNow;

        _solver.Solve();
    }
}
