using System.ComponentModel.DataAnnotations;

namespace BridgertonGame.Server.Data.Entities;

public class ChatMessage
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string SenderId { get; set; } = string.Empty;
    
    [Required]
    public string SenderName { get; set; } = string.Empty;
    
    [Required]
    public string FamilyName { get; set; } = string.Empty;
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}
