using Microsoft.AspNetCore.SignalR;
using BridgertonGame.Shared.Models;

namespace BridgertonGame.Server.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(ChatMessage message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }

    public async Task NotifyMessageDeleted()
    {
        await Clients.All.SendAsync("MessagesCleared");
    }
}
