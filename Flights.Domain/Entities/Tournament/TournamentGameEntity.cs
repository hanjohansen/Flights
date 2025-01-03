using Flights.Domain.Entities.Game;

namespace Flights.Domain.Entities.Tournament;

public class TournamentGameEntity : BaseEntity
{
    public Guid TournamentRoundId { get; set; }

    public TournamentRoundEntity TournamentRound { get; set; } = null!;
    
    public int OrderNumber {get;set;}
    
    public bool IsLosersCup {get;set;}
    
    public GameEntity? Game { get; set; }
}