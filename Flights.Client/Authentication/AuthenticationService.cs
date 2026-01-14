using Flights.Client.Configuration;
using Flights.Client.Configuration.Logging;
using Flights.Client.Service.Port;
using Flights.Domain.Exception;
using Flights.Infrastructure.Port;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using Serilog.Context;

namespace Flights.Client.Authentication;

public interface IAuthenticationService
{
    FlightsAuthenticationStateProvider AuthStateProvider { get; }
    AuthenticatedTenant? Tenant { get; }
    
    Task Authenticate(string username, string password);

    Task TryReauthenticate();

    Task TryAutoAuthenticate();
    
    Task Unauthenticate();
}

public class AuthenticationService(
    AuthenticationStateProvider authStateProvider, 
    IOptions<AuthConfiguration> authConfiguration,
    ITenantRepository tenantRepo, 
    IHashingService hashService, 
    IBrowserStorage browserStorage, 
    IUserSessionCache sessionCache,
    ILogger<AuthenticationService> logger) : IAuthenticationService
{
    public FlightsAuthenticationStateProvider AuthStateProvider { get; } = (FlightsAuthenticationStateProvider)authStateProvider;

    private const string SessionStoreKey = "SessionId";

    public AuthenticatedTenant? Tenant => AuthStateProvider.AuthenticatedTenant;
    
    public async Task Authenticate(string userName, string password)
    {
        var tenant = await tenantRepo.GetTenantByName(userName);

        if (tenant == null)
        {
            DoLog(null, "Failed login attempt to tenant '{0}' => tenant not found", userName);
            throw new FlightsAuthException("Invalid user or password");
        }

        var pwHash = tenant.Password;
        var match = hashService.MatchHash(password, pwHash);

        if (!match)
        {
            DoLog(null, "Failed login attempt to tenant '{0}' => invalid password", userName);
            throw new FlightsAuthException("Invalid user or password");
        }
        
        DoLog(tenant.Id, "Tenant '{0}' logged in using password", tenant.Name);

        await AuthStateProvider.Login(tenant.Id, tenant.Name);
        await browserStorage.SetBrowserItem(SessionStoreKey, tenant.Id, TimeSpan.FromDays(3));
    }

    public async Task TryReauthenticate()
    {
        //check if sessionId exists in browser storage
        var sessionId = await browserStorage.GetAndTouchBrowserItem<Guid?>(SessionStoreKey);
        if (sessionId == null)
            return;

        //check if active session exists for id
        if (!sessionCache.HasUserSession(sessionId.Value))
            return;
        
        //reauth tenant
        var tenant = await tenantRepo.GetTenantById(sessionId.Value);
        DoLog(tenant.Id, "Tenant '{0}' logged in using session-refresh", tenant.Name);
        
        await AuthStateProvider.Login(tenant.Id, tenant.Name);
    }

    public async Task TryAutoAuthenticate()
    {
        if (!authConfiguration.Value.AllowAutoLogin)
            return;

        var tenantIds = await tenantRepo.GetAllTenantIds();

        if (tenantIds.Count != 1)
            return;

        var tenantId = tenantIds.Single();
        var tenant = await tenantRepo.GetTenantById(tenantId);
        
        DoLog(tenant.Id, "Tenant '{0}' logged in using auto-login", tenant.Name);
        
        await AuthStateProvider.Login(tenant.Id, tenant.Name);
        await browserStorage.SetBrowserItem(SessionStoreKey, tenant.Id, TimeSpan.FromDays(3));
    }

    public async Task Unauthenticate()
    {
        if (Tenant == null)
            return;
        
        var tenant = Tenant.Name;
        var id = Tenant.Id;
        
        AuthStateProvider.Logout();
        await browserStorage.RemoveBrowserItem(SessionStoreKey);
        DoLog(id, "Tenant '{0}' logged out", tenant);
    }

    private void DoLog(Guid? tenantId, string message, params object[] args)
    {
        var tId = tenantId?.ToString() ?? "";
        using(LogContext.PushProperty("TenantId", tId))
            logger.LogInformation(message, args);
    }
}