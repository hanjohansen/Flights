using Microsoft.AspNetCore.SignalR;

namespace Flights.Client.Rtc;

public enum UiEventType { LoadedIndex, LoadedGame, UpdatedGame, LoadedTournament, UpdatedTournament}

public record RtcUiMessage(UiEventType Type, Guid Id);

public class RtcHub : Hub
{
    public const string HubUrl = "/viewer-hub";
    public const string SendBroadcast = nameof(Broadcast);
    public const string GetBroadcast = "ReceiveBroadcast";

    public async Task Broadcast(RtcUiMessage message)
    {
        await Clients.All.SendAsync(GetBroadcast, message);
    }
}