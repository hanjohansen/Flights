using Flights.Domain.Entities.Game;

namespace Flights.Domain.Entities.Tournament;

public class TournamentEntity : BaseEntity
{
    public GameType Type { get; set;}
    
    public bool SemiFinalWithLosersCup { get; set; }

    public int X01Target {get;set;} = 301;

    public InOutModifier InModifier {get;set;}
    
    public InOutModifier OutModifier {get;set;}

    public DateTimeOffset Started {get;set;}

    public DateTimeOffset? Finished {get;set;}

    public List<TournamentPlayerEntity> Players { get; set; } = new();

    public List<TournamentRoundEntity> Rounds { get; set; } = new();
}