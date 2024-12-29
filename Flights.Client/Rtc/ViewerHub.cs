using Microsoft.AspNetCore.SignalR;

namespace Flights.Client.Rtc;

public enum ViewerHubMessageType { LoadedGame, UpdatedGame, LoadedTournament, UpdatedTournament}

public record ViewerHubMessage(ViewerHubMessageType Type, Guid Id);

public class ViewerHub : Hub
{
    public const string HubUrl = "/viewer-hub";
    public const string SendBroadcast = nameof(Broadcast);
    public const string GetBroadcast = "ReceiveBroadcast";

    public async Task Broadcast(ViewerHubMessage message)
    {
        await Clients.All.SendAsync(GetBroadcast, message);
    }
}