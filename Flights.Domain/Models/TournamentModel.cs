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
    
    public static TournamentModel Create(List<PlayerEntity> players, GameType type, int x01Target, InOutModifier inMod, InOutModifier outMod)
    {
        if (players.Count < 4)
            throw new FlightsGameException("Minimum of four players required for tournament!");
        
        var tournament = new TournamentEntity()
        {
            Type = type,
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

    public static TournamentModel Init(TournamentEntity entity) => new(entity);

    private TournamentModel(TournamentEntity entity)
    {
        Entity = entity;
        _solver = GameSolverFactory.GetTournamentSolver(entity);
    }

    public TournamentState ResolveTournamentState()
    {
        return _solver.Solve();
    }
}
