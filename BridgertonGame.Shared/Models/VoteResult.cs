namespace BridgertonGame.Shared.Models;

public class VoteDetails
{
    public string VoterId { get; set; } = string.Empty;
    public string VoterName { get; set; } = string.Empty;
    public string VotedForName { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int PointsAwarded { get; set; }
}

public class FamilyVoteResult
{
    public string FamilyId { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public string? ActualLadyWhistledownName { get; set; }
    public List<VoteDetails> Votes { get; set; } = new();
    public int TotalCorrectVotes { get; set; }
    public int TotalIncorrectVotes { get; set; }
    public int TotalPointsAwarded { get; set; }
    public bool IsRevealed { get; set; }
}
