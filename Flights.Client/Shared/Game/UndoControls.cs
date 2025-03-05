using Flights.Domain.Models;

namespace Flights.Client.Shared.Game;

public class UndoControls
{
    public required List<StatModel> UndoStack { get; set; }
    
    public required Func<Task>? RequestUndoDart {get;set;}

    public required Func<Task>? RequestRedoDart {get;set;}

    public required Func<Task>? RequestAbortUndo {get;set;}
}