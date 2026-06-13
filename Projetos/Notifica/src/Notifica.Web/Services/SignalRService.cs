using Microsoft.AspNetCore.SignalR.Client;
using Notifica.Web.Models;

namespace Notifica.Web.Services;

public class SignalRService
{
    private HubConnection? _hub;

    public event Action<NotificationDto>? OnNotificationReceived;

    public async Task ConnectAsync(string token, string baseUrl)
    {
        _hub = new HubConnectionBuilder()
            .WithUrl($"{baseUrl}/hubs/notifications?access_token={token}")
            .WithAutomaticReconnect()
            .Build();

        _hub.On<NotificationDto>("ReceiveNotification", notification =>
        {
            OnNotificationReceived?.Invoke(notification);
        });

        await _hub.StartAsync();
    }

    public async Task DisconnectAsync()
    {
        if (_hub is not null)
            await _hub.DisposeAsync();
    }
}
