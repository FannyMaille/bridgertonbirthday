using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BridgertonGame.Server.Hubs;

namespace BridgertonGame.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationsController(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpPost("test")]
    public async Task<ActionResult> SendTestNotification([FromBody] TestNotificationRequest request)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveNotification",
            request.Title ?? "🧪 Notification de test",
            request.Message ?? "Ceci est une notification de test",
            request.Type ?? "info",
            null,
            null);

        return Ok(new { success = true, message = "Notification envoyée" });
    }

    [HttpPost("article-test")]
    public async Task<ActionResult> SendTestArticleNotification()
    {
        await _hubContext.Clients.All.SendAsync("ReceiveNotification",
            "📰 Nouvelle Chronique !",
            "Lady Whistledown de la famille Test vient de publier une chronique mondaine.",
            "article",
            "test-article-id",
            "Famille Test");

        return Ok(new { success = true, message = "Notification d'article envoyée" });
    }
}

public class TestNotificationRequest
{
    public string? Title { get; set; }
    public string? Message { get; set; }
    public string? Type { get; set; }
}
