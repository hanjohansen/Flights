using Flights.Domain.Entities.Game;
using Flights.Domain.Exception;

namespace Flights.Domain.Models;
public record StatModel(DartModifier Modifier, int Value){
    public static StatModel Init(int value){
        return new StatModel(DartModifier.None, value);
    }

    public static StatModel Init(DartModifier modifier, int value){
        return new StatModel(modifier, value);
    }
    
    public void Validate(){
        if((Value < 0 || Value > 20) && Value != 25)
            throw new FlightsGameException("Invalid value (" + Value + ")!");

        if(Value == 0 && 
        (Modifier == DartModifier.Double || Modifier == DartModifier.Triple))
            throw new FlightsGameException("0 can not be modified (" + Modifier + ")!");

        if(Value == 25 && Modifier == DartModifier.Triple)
            throw new FlightsGameException("Bulls can not be triple-modified");
        
        if(Value == 26 && Modifier != DartModifier.None)
            throw new FlightsGameException("Waschmaschine can not be modified (" + Modifier + ")!");
    }
}