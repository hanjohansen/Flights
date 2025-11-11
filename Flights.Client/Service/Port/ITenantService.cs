namespace Flights.Client.Service.Port;

public interface ITenantService
{
    Task RenameTenant(string newName);
    
    Task ChangeTenantPassword(string oldPassword, string newPassword);
}