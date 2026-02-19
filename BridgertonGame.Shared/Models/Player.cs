namespace BridgertonGame.Shared.Models;

public class Player
{
    public string Id { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string FamilyId { get; set; } = string.Empty;
    public bool IsLadyWhistledown { get; set; }
}
