using Flights.Client.Shared.Game;
using Flights.Domain.State;

namespace Flights.Client.Shared.Tournament;

public class TournamentControls
{
    public required DisplayType DisplayType { get; set; }
    
    public required TournamentState CurrentTournament {get;set;}
    
    public required Func<Guid, Task>? SwitchPlayerOrder { get; set; }
    
    public required Func<Guid, Task>? DevFinishGame { get; set; }
    
    public required Func<Task>? SkipLosersCup { get; set; }
    
    public required Func<Guid,Guid,Task>? AddPlayerToGame { get; set; }

}