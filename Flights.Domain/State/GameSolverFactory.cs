using Flights.Domain.Entities.Game;
using Flights.Domain.Entities.Tournament;
using Flights.Domain.Exception;
using Flights.Domain.State.Solvers;
using Flights.Domain.State.Solvers.AroundTheClock;
using Flights.Domain.State.Solvers.X01;
using Flights.Domain.State.Solvers.Cricket;
using Flights.Domain.State.Solvers.Tournament;

namespace Flights.Domain.State;
public static class GameSolverFactory
{
    public static IGameSolver GetGameSolver(GameEntity entity){
        switch(entity.Type){
            case GameType.X01:
                return new X01Solver(entity);
            case GameType.Cricket:
            case GameType.CtCricket:
                return new CricketSolver(entity);
            case GameType.AroundTheClock:
                return new AroundTheClockSolver(entity);
        }

        throw new FlightsGameException("Game type not supported yet");      
    }

    public static ITournamentSolver GetTournamentSolver(TournamentEntity entity)
    {
        return new TournamentSolver(entity);
    }
}