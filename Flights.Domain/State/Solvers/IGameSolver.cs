namespace Flights.Domain.State.Solvers;
public interface IGameSolver
{
    GameState Solve();

    GameState FinalizeGame();
}