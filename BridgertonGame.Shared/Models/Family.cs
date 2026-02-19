namespace BridgertonGame.Shared.Models;

public class Family
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Points { get; set; }
    public int Rank { get; set; }
    public bool VotingEnabled { get; set; }
    public bool Revealed { get; set; }
    public string? LadyWhistledownId { get; set; }
}
