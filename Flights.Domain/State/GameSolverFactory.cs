using Flights.Domain.Entities;
using Flights.Domain.Exceptions;
using Flights.Domain.State.Solvers;

namespace Flights.Domain.State;
public class GameSolverFactory
{
    public static IGameSolver GetGameSolver(GameEntity entity){
        if(entity.Type == GameType.X01){
            return new X01Solver(entity);
        }

        throw new FlightsGameException("Game type not supported yet");      
    }
}