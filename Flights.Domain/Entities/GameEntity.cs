namespace Flights.Domain.Entities;

public enum GameType {X01, Cricket, CtCricket, AroundTheClock}
public enum InOutModifier {None, Double, Triple, FullBull}

public class GameEntity : BaseEntity
{
    public GameType Type { get; set;}

    public int X01Target {get;set;} = 301;

    public InOutModifier InModifier {get;set;}
    
    public InOutModifier OutModifier {get;set;}

    public DateTimeOffset Started {get;set;}

    public DateTimeOffset? Finished {get;set;}

    public List<GamePlayerEntity> Players { get; set;} = new();

    public List<GameRoundEntity> Rounds {get;set;} = new();
}