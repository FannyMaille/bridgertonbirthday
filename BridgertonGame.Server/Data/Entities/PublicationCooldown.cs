using System.ComponentModel.DataAnnotations;

namespace BridgertonGame.Server.Data.Entities;

public class PublicationCooldown
{
    [Key]
    public string FamilyId { get; set; } = string.Empty;
    public DateTime LastPublicationTime { get; set; }
}
