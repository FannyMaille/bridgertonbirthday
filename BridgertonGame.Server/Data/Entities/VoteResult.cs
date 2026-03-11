namespace BridgertonGame.Server.Data.Entities;

public class VoteResult
{
    public int Id { get; set; }
    public string FamilyId { get; set; } = string.Empty;
    public int CorrectVotes { get; set; } // Number of people who voted correctly
    public int IncorrectVotes { get; set; } // Number of people who voted incorrectly
    public int PointsAwarded { get; set; } // Net points awarded (+10 per correct, -10 per incorrect)
    public DateTime RevealedAt { get; set; }
}
