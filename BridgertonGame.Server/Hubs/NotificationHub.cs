using Microsoft.AspNetCore.SignalR;

namespace BridgertonGame.Server.Hubs;

public class NotificationHub : Hub
{
    public async Task SendNotification(string title, string message, string type)
    {
        await Clients.All.SendAsync("ReceiveNotification", title, message, type);
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        Console.WriteLine($"Client connected: {Context.ConnectionId}");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
        Console.WriteLine($"Client disconnected: {Context.ConnectionId}");
    }
}
