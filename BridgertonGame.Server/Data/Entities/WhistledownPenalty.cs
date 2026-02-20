using System.ComponentModel.DataAnnotations;

namespace BridgertonGame.Server.Data.Entities;

public class WhistledownPenalty
{
    [Key]
    public string FamilyId { get; set; } = string.Empty;
    public int Penalty { get; set; }
}
