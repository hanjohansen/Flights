using Flights.Domain.Entities.Game;

namespace Flights.Domain.Entities.Tournament;

public class TournamentEntity : BaseEntity
{
    public Guid TenantId { get; set; }
    public TenantEntity Tenant { get; set; } = null!;
    
    public int TournamentNumber {get;set;}
    
    public GameType Type {get; set;}

    public int FirstRoundPlayersPerGame {get;set;} = 2;
    
    public bool SemiFinalWithLosersCup {get; set;}

    public int X01Target {get;set;} = 301;

    public InOutModifier InModifier {get;set;}
    
    public InOutModifier OutModifier {get;set;}

    public DateTimeOffset Started {get;set;}

    public DateTimeOffset? Finished {get;set;}

    public List<TournamentPlayerEntity> Players { get; set; } = new();

    public List<TournamentRoundEntity> Rounds { get; set; } = new();
}