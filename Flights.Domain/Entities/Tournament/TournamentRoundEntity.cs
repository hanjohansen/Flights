namespace Flights.Domain.Entities.Tournament;

public class TournamentRoundEntity : BaseEntity
{
    public int OrderNumber {get;set;}
    
    public Guid TournamentId { get; set; }
    public TournamentEntity Tournament { get; set; } = null!;
    
    public Guid? WildCardId { get; set; }
    public TournamentPlayerEntity? WildCard { get; set; }
}