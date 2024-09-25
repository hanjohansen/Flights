namespace Flights.Domain.Exceptions;
public class FlightsGameException : Exception
{
    public FlightsGameException(string message) : base(message){}
}