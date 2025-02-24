using Flights.Domain.Models;
using Flights.Domain.State;

namespace Flights.Client.Shared.Game;

public enum DisplayType
{
    Normal,
    Viewer
};

public class GameControls
{
    public required DisplayType DisplayType { get; set; }
    
    public required GameState CurrentGame {get;set;}

    public required Func<StatModel, Task>? ReportScore {get;set;}

    public required Func<Task>? ReportNullDart {get;set;}

    public required Func<Task>? RequestRevertDart {get;set;}
}