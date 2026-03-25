using Microsoft.AspNetCore.Mvc;
using BridgertonGame.Server.Services;
using BridgertonGame.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using BridgertonGame.Server.Hubs;

namespace BridgertonGame.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly DatabaseGameDataService _dataService;
    private readonly IHubContext<ChatHub> _chatHub;

    public ChatController(DatabaseGameDataService dataService, IHubContext<ChatHub> chatHub)
    {
        _dataService = dataService;
        _chatHub = chatHub;
    }

    [HttpGet]
    public async Task<ActionResult<List<ChatMessage>>> GetMessages()
    {
        var messages = await _dataService.GetAllChatMessagesAsync();
        return Ok(messages);
    }

    [HttpPost]
    public async Task<ActionResult<ChatMessage>> SendMessage([FromBody] SendChatMessageRequest request)
    {
        try
        {
            var message = await _dataService.SendChatMessageAsync(request.SenderId, request.Content);
            await _chatHub.Clients.All.SendAsync("ReceiveMessage", message);
            return Ok(message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteAllMessages()
    {
        var deleted = await _dataService.DeleteAllChatMessagesAsync();
        if (deleted)
        {
            await _chatHub.Clients.All.SendAsync("MessagesCleared");
            return Ok(new { message = "Tous les messages ont été supprimés" });
        }
        return Ok(new { message = "Aucun message à supprimer" });
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetMessageCount()
    {
        var count = await _dataService.GetChatMessageCountAsync();
        return Ok(count);
    }
}
