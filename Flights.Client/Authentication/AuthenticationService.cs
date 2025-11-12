using Flights.Client.Service.Port;
using Flights.Domain.Exception;
using Flights.Infrastructure.Port;
using Microsoft.AspNetCore.Components.Authorization;

namespace Flights.Client.Authentication;

public interface IAuthenticationService
{
    FlightsAuthenticationStateProvider AuthStateProvider { get; }
    AuthenticatedTenant? Tenant { get; }
    
    Task Authenticate(string username, string password);

    Task TryReauthenticate();
    
    Task Unauthenticate();
}

public class AuthenticationService(AuthenticationStateProvider authStateProvider, ITenantRepository tenantRepo, IHashingService hashService, IBrowserStorage browserStorage, IUserSessionCache sessionCache) : IAuthenticationService
{
    public FlightsAuthenticationStateProvider AuthStateProvider { get; } = (FlightsAuthenticationStateProvider)authStateProvider;

    private const string SessionStoreKey = "SessionId";

    public AuthenticatedTenant? Tenant => AuthStateProvider.AuthenticatedTenant;
    
    public async Task Authenticate(string userName, string password)
    {
        var tenant = await tenantRepo.GetTenantByName(userName);
        
        if(tenant == null)
            throw new FlightsAuthException("Tenant not found");

        var pwHash = tenant.Password;
        var match = hashService.MatchHash(password, pwHash);
        
        if(!match)
            throw new FlightsAuthException("Invalid password");
        
        await AuthStateProvider.Login(tenant.Id, tenant.Name);
        await browserStorage.SetSessionItem(SessionStoreKey, tenant.Id);
    }

    public async Task TryReauthenticate()
    {
        //check if sessionId exists in browser storage
        var sessionId = await browserStorage.GetSessionItem<Guid?>(SessionStoreKey);
        if (sessionId == null)
            return;

        //check if active session exists for id
        if (!sessionCache.HasUserSession(sessionId.Value))
            return;
        
        //reauth tenant
        var tenant = await tenantRepo.GetTenantById(sessionId.Value);
        await AuthStateProvider.Login(tenant.Id, tenant.Name);
    }

    public async Task Unauthenticate()
    {
        AuthStateProvider.Logout();
        await browserStorage.RemoveSessionItem(SessionStoreKey);
    }
}