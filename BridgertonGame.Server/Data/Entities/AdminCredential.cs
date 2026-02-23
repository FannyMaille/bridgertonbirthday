using System.ComponentModel.DataAnnotations;

namespace BridgertonGame.Server.Data.Entities;

public class AdminCredential
{
    [Key]
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
