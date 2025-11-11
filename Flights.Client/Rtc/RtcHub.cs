using Microsoft.AspNetCore.SignalR;

namespace Flights.Client.Rtc;

public enum UiEventType { LoadedIndex, LoadedGame, UpdatedGame, LoadedTournament, UpdatedTournament}

public record RtcUiMessage(UiEventType Type, Guid Id);

public class RtcHub : Hub
{
    public const string HubUrl = "/viewer-hub";
    public const string SendBroadcast = nameof(Broadcast);
    public const string GetBroadcast = "ReceiveBroadcast";

    public override async Task OnConnectedAsync()
    {
        var authUserId = TryGetTenantIdFromHeaders();

        if (authUserId != null)
            await Groups.AddToGroupAsync(Context.ConnectionId, authUserId.ToString() ?? "");
        
        await base.OnConnectedAsync();
    }
    
    public async Task Broadcast(RtcUiMessage message)
    {
        var authUserId  = TryGetTenantIdFromHeaders();
        
        if (authUserId != null)
            await Clients.Group(authUserId.ToString() ?? "").SendAsync(GetBroadcast, message);
    }

    private Guid? TryGetTenantIdFromHeaders()
    {
        var headers = Context.GetHttpContext()?.Request.Headers;
        var tenantIdHeader = headers?.FirstOrDefault(x => x.Key == "x-tenant-id");
        var tenantIdString = tenantIdHeader?.Value.FirstOrDefault();

        if (tenantIdString == null)
            return null;

        if (Guid.TryParse(tenantIdString, out Guid id))
            return id;
        
        return null;
    }
}