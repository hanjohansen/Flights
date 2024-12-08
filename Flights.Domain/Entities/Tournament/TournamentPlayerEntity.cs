namespace Flights.Domain.Entities.Tournament;

public class TournamentPlayerEntity : BaseEntity
{
    public Guid TournamentId {get;set;}

    public TournamentEntity Tournament {get;set;} = null!;

    public Guid PlayerId {get;set;}

    public PlayerEntity Player {get;set;} = null!;

    public int OrderNumber {get;set;}
}