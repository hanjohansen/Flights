using Flights.Client.Authentication;
using Flights.Client.Service.Port;
using Flights.Domain.Exception;
using Flights.Infrastructure.Port;

namespace Flights.Client.Service;

public class TenantService(IAuthenticationService authService, ITenantRepository tenantRepo, IHashingService hashService) : ITenantService
{
    public async Task RenameTenant(string newName)
    {
        var tenantId = GetTenantId();
        
        await tenantRepo.ChangeTenantName(tenantId, newName);
        
        authService.AuthStateProvider.ChangeAuthenticatedTenantName(newName);
    }

    public async Task ChangeTenantPassword(string oldPassword, string newPassword)
    {
        var tenantId = GetTenantId();

        var tenant = await tenantRepo.GetTenantById(tenantId);
        
        bool pwMatch = hashService.MatchHash(oldPassword, tenant.Password);

        if (!pwMatch)
            throw new FlightsAuthException("Old password is invalid");

        var hashed = hashService.CreateHash(newPassword);
        
        await tenantRepo.ChangeTenantPassword(tenantId, hashed);
    }

    private Guid GetTenantId()
    {
        var id = authService.Tenant?.Id;

        if (id == null)
            throw new FlightsAuthException("Tenant is not authenticated!");

        return id.Value;
    }
}