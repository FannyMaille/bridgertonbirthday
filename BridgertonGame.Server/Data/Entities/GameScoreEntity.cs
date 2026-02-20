using System.ComponentModel.DataAnnotations;

namespace BridgertonGame.Server.Data.Entities;

public class GameScoreEntity
{
    [Key]
    public int Id { get; set; }
    public string GameName { get; set; } = string.Empty;
    public string FamilyId { get; set; } = string.Empty;
    public int Score { get; set; }
}
