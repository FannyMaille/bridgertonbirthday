namespace BridgertonGame.Shared.Models;

public class Notification
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = "info"; // info, success, warning, article
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? ArticleId { get; set; }
    public string? FamilyName { get; set; }
    public bool IsRead { get; set; } = false;
}
