namespace BridgertonGame.Shared.Models;

public class ChatMessage
{
    public int Id { get; set; }
    public string SenderId { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
}

public class SendChatMessageRequest
{
    public string SenderId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
