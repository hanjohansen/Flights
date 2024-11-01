namespace Flights.Domain.Exception;
public class FlightsGameException : System.Exception
{
    public FlightsGameException(string message) : base(message){}
}