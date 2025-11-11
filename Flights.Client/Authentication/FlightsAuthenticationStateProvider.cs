using System.Security.Claims;
using Flights.Infrastructure.Port;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;

namespace Flights.Client.Authentication;

public record AuthenticatedTenant(Guid Id, string Name);

public class FlightsAuthenticationStateProvider(IUserSessionCache sessionCache, ILoggerFactory loggerFactory) : RevalidatingServerAuthenticationStateProvider(loggerFactory)
{
    public AuthenticatedTenant? AuthenticatedTenant { get; private set; }
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var principal = GetPrincipalFromTenant();
        return await Task.FromResult(new AuthenticationState(principal));
    }
    
    protected override async Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
    {
        var idClaim = authenticationState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (idClaim == null)
            return false;
        
        var userId = Guid.Parse(idClaim.Value);
        var hasSession = sessionCache.HasUserSession(userId);

        return await Task.FromResult(hasSession);
    }

    public async Task Login(Guid userId, string userName)
    {
        await sessionCache.CacheUserSession(userId);
        AuthenticatedTenant = new AuthenticatedTenant(userId, userName);
        
        var principal = GetPrincipalFromTenant();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    public void ChangeAuthenticatedTenantName(string newName)
    {
        if (AuthenticatedTenant == null)
            return;
        
        AuthenticatedTenant = AuthenticatedTenant with { Name = newName };
        var principal = GetPrincipalFromTenant();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }
    
    public void Logout()
    {
        if (AuthenticatedTenant == null)
            return;
        
        sessionCache.RemoveUserSession(AuthenticatedTenant.Id);
        AuthenticatedTenant = null;
        
        var principal = GetPrincipalFromTenant();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    private ClaimsPrincipal GetPrincipalFromTenant()
    {
        if(AuthenticatedTenant == null)
            return new ClaimsPrincipal(new List<ClaimsIdentity>());
        
        return new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, AuthenticatedTenant.Id.ToString()),
            new Claim(ClaimTypes.Name, AuthenticatedTenant.Name),
            new Claim(ClaimTypes.Role, "user")
        }, "FlightsAuth"));
    }
    
    protected override TimeSpan RevalidationInterval => TimeSpan.FromSeconds(10);
}