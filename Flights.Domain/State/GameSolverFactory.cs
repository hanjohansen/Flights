using Flights.Domain.Entities;
using Flights.Domain.Exception;
using Flights.Domain.State.Solvers;
using Flights.Domain.State.Solvers.X01;
using Flights.Domain.State.Solvers.Cricket;
using Flights.Domain.State.Solvers.Shanghai;

namespace Flights.Domain.State;
public class GameSolverFactory
{
    public static IGameSolver GetGameSolver(GameEntity entity){
        switch(entity.Type){
            case GameType.X01:
                return new X01Solver(entity);
            case GameType.Cricket:
            case GameType.CtCricket:
                return new CricketSolver(entity);
            case GameType.Shanghai:
                return new ShanghaiSolver(entity);
        }

        throw new FlightsGameException("Game type not supported yet");      
    }
}