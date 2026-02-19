namespace BridgertonGame.Shared.Models;

public class GameScore
{
    public string GameName { get; set; } = string.Empty;
    public Dictionary<string, int> FamilyScores { get; set; } = new();
}
