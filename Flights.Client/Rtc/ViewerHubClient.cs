using Microsoft.AspNetCore.SignalR.Client;

namespace Flights.Client.Rtc;

public class ViewerHubClient : IAsyncDisposable
{
    private HubConnection? _hubConnection;

    public delegate void GameEventHandler(ViewerHubMessage message);

    public event GameEventHandler? GameEventReceived;
    
    public async Task Connect(string baseUrl)
    {
        var uri = baseUrl.TrimEnd('/') + ViewerHub.HubUrl;
        
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(uri!)
            .Build();
        
        _hubConnection.On<ViewerHubMessage>(ViewerHub.GetBroadcast, OnGameEventReceived);

        await _hubConnection.StartAsync();
    }

    public async Task PublishGameEvent(ViewerHubMessageType type, Guid id)
    {
        if (_hubConnection != null)
            await _hubConnection.SendAsync(ViewerHub.SendBroadcast, new ViewerHubMessage(type, id));
    }

    protected virtual void OnGameEventReceived(ViewerHubMessage message)
    {
        GameEventReceived?.Invoke(message);
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
        }
    }
}