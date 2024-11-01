namespace Flights.Domain.Entities;

public enum DartModifier {None, Double, Triple}
public class DartStatEntity
{    
    public DartModifier Modifier { get;set;}

    public int Value { get; set;}

    public int GetCalculatedValue(){
        switch(Modifier){
            case DartModifier.Double:
                return Value * 2;
            case DartModifier.Triple:
                return Value * 3;
            case DartModifier.None:
            default:
                return Value;
        }
    }
}