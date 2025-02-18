using Flights.Domain.Entities.Game;
using Flights.Domain.Models;
using Flights.Domain.State;

namespace Flights.Infrastructure.Port;

public interface ITournamentRepository
{
    Task<TournamentState> CreateTournament(List<Guid> players, int firstRoundPlayersPerGame, GameType type, bool semiFinalWithLosersCup, int x01Target, InOutModifier inMod,
        InOutModifier outMod);

    Task<TournamentModel> GetTournament(Guid id);

    Task<TournamentState> ReplayTournament(Guid id);

    Task DeleteTournament(Guid id);

    Task<TournamentState> SwitchPlayerOrder(Guid tournamentId, Guid gameId);
    
    Task<TournamentState> SkipLosersCup(Guid tournamentId);

    Task<TournamentState> DevFinishGame(Guid tournamentId, Guid gameId);
}