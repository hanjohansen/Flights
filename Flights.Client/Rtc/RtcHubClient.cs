using Microsoft.AspNetCore.SignalR.Client;

namespace Flights.Client.Rtc;

public class RtcHubClient : IAsyncDisposable
{
    private HubConnection? _hubConnection;

    public delegate Task GameEventHandler(RtcUiMessage message);

    public event GameEventHandler? GameEventReceived;
    
    public async Task Connect(string baseUrl)
    {
        var uri = baseUrl.TrimEnd('/') + RtcHub.HubUrl;
        
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(uri)
            .Build();
        
        _hubConnection.On<RtcUiMessage>(RtcHub.GetBroadcast, OnGameEventReceived);

        await _hubConnection.StartAsync();
    }

    public async Task PublishGameEvent(UiEventType type, Guid id)
    {
        if (_hubConnection != null)
            await _hubConnection.SendAsync(RtcHub.SendBroadcast, new RtcUiMessage(type, id));
    }

    protected virtual void OnGameEventReceived(RtcUiMessage message)
    {
        GameEventReceived?.Invoke(message);
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection != null)
        {
            if(GameEventReceived != null)
                GameEventReceived = (GameEventHandler) Delegate.RemoveAll(GameEventReceived, GameEventReceived)!;

            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
        }
    }
}