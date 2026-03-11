namespace BridgertonGame.Server.Data.Entities;

public class Vote
{
    public int Id { get; set; }
    public string FamilyId { get; set; } = string.Empty;
    public string VoterId { get; set; } = string.Empty; // Player who voted
    public string VotedForId { get; set; } = string.Empty; // Player who was voted for
    public DateTime VotedAt { get; set; }
}
