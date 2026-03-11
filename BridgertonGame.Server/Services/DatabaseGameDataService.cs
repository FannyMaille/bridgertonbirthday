using Microsoft.EntityFrameworkCore;
using BridgertonGame.Shared.Models;
using BridgertonGame.Server.Data;
using BridgertonGame.Server.Data.Entities;

namespace BridgertonGame.Server.Services;

public class DatabaseGameDataService
{
    private readonly BridgertonDbContext _context;

    public DatabaseGameDataService(BridgertonDbContext context)
    {
        _context = context;
    }

    // Player methods
    public async Task<Player?> GetPlayerByCodeAsync(string code)
    {
        return await _context.Players
            .FirstOrDefaultAsync(p => p.Code.ToLower() == code.Trim().ToLower());
    }

    public async Task<List<Player>> GetAllPlayersAsync()
    {
        return await _context.Players.ToListAsync();
    }

    public async Task<List<Player>> GetPlayersByFamilyAsync(string familyId)
    {
        return await _context.Players
            .Where(p => p.FamilyId == familyId)
            .ToListAsync();
    }

    public async Task<bool> UpdatePlayerAsync(Player player)
    {
        var existingPlayer = await _context.Players.FindAsync(player.Id);
        if (existingPlayer == null)
            return false;

        existingPlayer.Name = player.Name;
        existingPlayer.Title = player.Title;
        existingPlayer.Code = player.Code;
        existingPlayer.Role = player.Role;
        existingPlayer.ImageUrl = player.ImageUrl;
        existingPlayer.FamilyId = player.FamilyId;
        existingPlayer.IsLadyWhistledown = player.IsLadyWhistledown;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddPlayerAsync(Player player)
    {
        try
        {
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeletePlayerAsync(string playerId)
    {
        var player = await _context.Players.FindAsync(playerId);
        if (player == null)
            return false;

        // Si c'est une Lady Whistledown, retirer la référence dans la famille
        if (player.IsLadyWhistledown)
        {
            var family = await _context.Families.FindAsync(player.FamilyId);
            if (family != null && family.LadyWhistledownId == playerId)
            {
                family.LadyWhistledownId = null;
            }
        }

        _context.Players.Remove(player);
        await _context.SaveChangesAsync();
        return true;
    }

    // Family methods
    public async Task<List<Family>> GetAllFamiliesAsync()
    {
        return await _context.Families.ToListAsync();
    }

    public async Task<Family?> GetFamilyByIdAsync(string id)
    {
        return await _context.Families.FindAsync(id);
    }

    public async Task UpdateFamilyPointsAsync(string familyId, int points)
    {
        var family = await _context.Families.FindAsync(familyId);
        if (family != null)
        {
            family.Points = points;
            await _context.SaveChangesAsync();
        }
    }

    public async Task SetLadyWhistledownAsync(string familyId, string playerId)
    {
        var family = await _context.Families.FindAsync(familyId);
        if (family != null)
        {
            family.LadyWhistledownId = playerId;
            await _context.SaveChangesAsync();
        }
    }

    public async Task ToggleVotingAsync(string familyId, bool enabled)
    {
        var family = await _context.Families.FindAsync(familyId);
        if (family != null)
        {
            family.VotingEnabled = enabled;
            await _context.SaveChangesAsync();
        }
    }

    public async Task RevealLadyWhistledownAsync(string familyId)
    {
        var family = await _context.Families.FindAsync(familyId);
        if (family != null)
        {
            family.Revealed = true;
            
            // Calculate and award points based on votes
            await CalculateAndAwardVotePointsAsync(familyId);
            
            await _context.SaveChangesAsync();
        }
    }

    public async Task ToggleRevealLadyWhistledownAsync(string familyId, bool revealed)
    {
        var family = await _context.Families.FindAsync(familyId);
        if (family != null)
        {
            // If revealing for the first time, calculate points
            if (revealed && !family.Revealed)
            {
                await CalculateAndAwardVotePointsAsync(familyId);
            }
            // If unrevealing, remove the vote points
            else if (!revealed && family.Revealed)
            {
                await RemoveVotePointsAsync(familyId);
            }
            
            family.Revealed = revealed;
            await _context.SaveChangesAsync();
        }
    }

    private async Task CalculateAndAwardVotePointsAsync(string familyId)
    {
        var family = await _context.Families.FindAsync(familyId);
        if (family == null || family.LadyWhistledownId == null)
            return;

        // Get all votes for this family
        var votes = await _context.Votes
            .Where(v => v.FamilyId == familyId)
            .ToListAsync();

        if (!votes.Any())
            return;

        int correctVotes = 0;
        int incorrectVotes = 0;

        foreach (var vote in votes)
        {
            if (vote.VotedForId == family.LadyWhistledownId)
            {
                correctVotes++;
            }
            else
            {
                incorrectVotes++;
            }
        }

        // Calculate net points: +10 for correct, -10 for incorrect
        int pointsAwarded = (correctVotes * 10) - (incorrectVotes * 10);

        // Create or update vote result
        var existingResult = await _context.VoteResults
            .FirstOrDefaultAsync(vr => vr.FamilyId == familyId);

        if (existingResult != null)
        {
            existingResult.CorrectVotes = correctVotes;
            existingResult.IncorrectVotes = incorrectVotes;
            existingResult.PointsAwarded = pointsAwarded;
            existingResult.RevealedAt = DateTime.UtcNow;
        }
        else
        {
            var voteResult = new VoteResult
            {
                FamilyId = familyId,
                CorrectVotes = correctVotes,
                IncorrectVotes = incorrectVotes,
                PointsAwarded = pointsAwarded,
                RevealedAt = DateTime.UtcNow
            };
            _context.VoteResults.Add(voteResult);
        }

        // Add points to family through game score
        await CreateOrUpdateVoteGameScoreAsync(familyId, pointsAwarded);
    }

    private async Task RemoveVotePointsAsync(string familyId)
    {
        // Remove vote result
        var voteResult = await _context.VoteResults
            .FirstOrDefaultAsync(vr => vr.FamilyId == familyId);
        
        if (voteResult != null)
        {
            _context.VoteResults.Remove(voteResult);
        }

        // Remove vote game score
        var voteScore = await _context.GameScores
            .FirstOrDefaultAsync(gs => gs.GameName == "Votes Lady Whistledown" && gs.FamilyId == familyId);
        
        if (voteScore != null)
        {
            _context.GameScores.Remove(voteScore);
        }
    }

    private async Task CreateOrUpdateVoteGameScoreAsync(string familyId, int points)
    {
        var scoreEntity = await _context.GameScores
            .FirstOrDefaultAsync(g => g.GameName == "Votes Lady Whistledown" && g.FamilyId == familyId);

        if (scoreEntity != null)
        {
            scoreEntity.Score = points;
        }
        else
        {
            var newScore = new GameScoreEntity
            {
                GameName = "Votes Lady Whistledown",
                FamilyId = familyId,
                Score = points
            };
            _context.GameScores.Add(newScore);
        }
    }

    public async Task<List<Vote>> GetAllVotesAsync()
    {
        return await _context.Votes.ToListAsync();
    }

    public async Task SaveVoteAsync(string familyId, string voterId, string votedForId)
    {
        // Check if user already voted
        var existingVote = await _context.Votes
            .FirstOrDefaultAsync(v => v.FamilyId == familyId && v.VoterId == voterId);

        if (existingVote != null)
        {
            // Update existing vote
            existingVote.VotedForId = votedForId;
            existingVote.VotedAt = DateTime.UtcNow;
        }
        else
        {
            // Create new vote
            var vote = new Vote
            {
                FamilyId = familyId,
                VoterId = voterId,
                VotedForId = votedForId,
                VotedAt = DateTime.UtcNow
            };
            _context.Votes.Add(vote);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteVoteAsync(string familyId, string voterId)
    {
        var vote = await _context.Votes
            .FirstOrDefaultAsync(v => v.FamilyId == familyId && v.VoterId == voterId);

        if (vote == null)
            return false;

        _context.Votes.Remove(vote);
        await _context.SaveChangesAsync();

        // If family is revealed, recalculate points
        var family = await _context.Families.FindAsync(familyId);
        if (family != null && family.Revealed)
        {
            await CalculateAndAwardVotePointsAsync(familyId);
            await _context.SaveChangesAsync();
        }

        return true;
    }

    public async Task<FamilyVoteResult> GetVoteResultsAsync(string familyId)
    {
        var family = await _context.Families.FindAsync(familyId);
        if (family == null)
            return new FamilyVoteResult();

        var votes = await _context.Votes
            .Where(v => v.FamilyId == familyId)
            .ToListAsync();

        var players = await _context.Players.ToListAsync();
        var ladyWhistledown = family.LadyWhistledownId != null 
            ? players.FirstOrDefault(p => p.Id == family.LadyWhistledownId) 
            : null;

        var voteDetails = votes.Select(v =>
        {
            var voter = players.FirstOrDefault(p => p.Id == v.VoterId);
            var votedFor = players.FirstOrDefault(p => p.Id == v.VotedForId);
            var isCorrect = v.VotedForId == family.LadyWhistledownId;

            return new VoteDetails
            {
                VoterId = v.VoterId,
                VoterName = voter?.Name ?? "Inconnu",
                VotedForName = votedFor?.Name ?? "Inconnu",
                IsCorrect = isCorrect,
                PointsAwarded = isCorrect ? 10 : -10
            };
        }).ToList();

        var voteResult = await _context.VoteResults
            .FirstOrDefaultAsync(vr => vr.FamilyId == familyId);

        return new FamilyVoteResult
        {
            FamilyId = familyId,
            FamilyName = family.Name,
            ActualLadyWhistledownName = ladyWhistledown?.Name,
            Votes = voteDetails,
            TotalCorrectVotes = voteResult?.CorrectVotes ?? 0,
            TotalIncorrectVotes = voteResult?.IncorrectVotes ?? 0,
            TotalPointsAwarded = voteResult?.PointsAwarded ?? 0,
            IsRevealed = family.Revealed
        };
    }

    public async Task<List<FamilyVoteResult>> GetAllVoteResultsAsync()
    {
        var families = await _context.Families.ToListAsync();
        var results = new List<FamilyVoteResult>();

        foreach (var family in families)
        {
            var result = await GetVoteResultsAsync(family.Id);
            results.Add(result);
        }

        return results;
    }

    public async Task CreateFamilyAsync(Family family)
    {
        _context.Families.Add(family);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateFamilyAsync(Family family)
    {
        var existingFamily = await _context.Families.FindAsync(family.Id);
        if (existingFamily == null)
            return false;

        existingFamily.Name = family.Name;
        existingFamily.Points = family.Points;
        existingFamily.Rank = family.Rank;
        existingFamily.VotingEnabled = family.VotingEnabled;
        existingFamily.Revealed = family.Revealed;
        existingFamily.LadyWhistledownId = family.LadyWhistledownId;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteFamilyAsync(string familyId)
    {
        // Check if family has any members
        var hasMembers = await _context.Players.AnyAsync(p => p.FamilyId == familyId);
        if (hasMembers)
            return false;

        var family = await _context.Families.FindAsync(familyId);
        if (family == null)
            return false;

        // Delete associated data
        var cooldown = await _context.PublicationCooldowns.FindAsync(familyId);
        if (cooldown != null)
            _context.PublicationCooldowns.Remove(cooldown);

        var penalty = await _context.WhistledownPenalties.FindAsync(familyId);
        if (penalty != null)
            _context.WhistledownPenalties.Remove(penalty);

        var articles = await _context.Articles.Where(a => a.FamilyId == familyId).ToListAsync();
        if (articles.Any())
            _context.Articles.RemoveRange(articles);

        var gameScores = await _context.GameScores.Where(g => g.FamilyId == familyId).ToListAsync();
        if (gameScores.Any())
            _context.GameScores.RemoveRange(gameScores);

        var votes = await _context.Votes.Where(v => v.FamilyId == familyId).ToListAsync();
        if (votes.Any())
            _context.Votes.RemoveRange(votes);

        var voteResults = await _context.VoteResults.Where(vr => vr.FamilyId == familyId).ToListAsync();
        if (voteResults.Any())
            _context.VoteResults.RemoveRange(voteResults);

        _context.Families.Remove(family);
        await _context.SaveChangesAsync();
        return true;
    }

    // Article methods
    public async Task<List<Article>> GetAllArticlesAsync()
    {
        return await _context.Articles
            .OrderByDescending(a => a.PublishedAt)
            .ToListAsync();
    }

    public async Task<List<Article>> GetArticlesByFamilyAsync(string familyId)
    {
        return await _context.Articles
            .Where(a => a.FamilyId == familyId)
            .OrderByDescending(a => a.PublishedAt)
            .ToListAsync();
    }

    public async Task<bool> CanPublishAsync(string familyId)
    {
        var cooldown = await _context.PublicationCooldowns.FindAsync(familyId);
        if (cooldown == null) return true;

        var diffMinutes = (DateTime.UtcNow - cooldown.LastPublicationTime).TotalMinutes;
        return diffMinutes >= 30;
    }

    public async Task<TimeSpan?> GetTimeUntilNextPublicationAsync(string familyId)
    {
        var cooldown = await _context.PublicationCooldowns.FindAsync(familyId);
        if (cooldown == null) return null;

        var nextTime = cooldown.LastPublicationTime.AddMinutes(30);
        var remaining = nextTime - DateTime.UtcNow;

        return remaining > TimeSpan.Zero ? remaining : null;
    }

    public async Task<Article> PublishArticleAsync(string title, string content, string familyId, string familyName)
    {
        var article = new Article
        {
            Id = Guid.NewGuid().ToString(),
            Title = title,
            Content = content,
            FamilyId = familyId,
            FamilyName = familyName,
            PublishedAt = DateTime.UtcNow
        };

        _context.Articles.Add(article);

        // Update or create cooldown
        var cooldown = await _context.PublicationCooldowns.FindAsync(familyId);
        if (cooldown == null)
        {
            cooldown = new PublicationCooldown
            {
                FamilyId = familyId,
                LastPublicationTime = DateTime.UtcNow
            };
            _context.PublicationCooldowns.Add(cooldown);
        }
        else
        {
            cooldown.LastPublicationTime = DateTime.UtcNow;
        }

        // Add automatic penalty of 10 points for publishing (family loses points)
        var penaltyEntity = await _context.WhistledownPenalties.FindAsync(familyId);
        if (penaltyEntity != null)
        {
            penaltyEntity.Penalty += 10; // Add 10 points to existing penalty
        }
        else
        {
            var newPenalty = new WhistledownPenalty
            {
                FamilyId = familyId,
                Penalty = 10
            };
            _context.WhistledownPenalties.Add(newPenalty);
        }

        await _context.SaveChangesAsync();
        return article;
    }

    public async Task DeleteArticleAsync(string articleId)
    {
        var article = await _context.Articles.FindAsync(articleId);
        if (article != null)
        {
            // Remove penalty associated with this article (10 points from family)
            var penaltyEntity = await _context.WhistledownPenalties.FindAsync(article.FamilyId);
            if (penaltyEntity != null && penaltyEntity.Penalty >= 10)
            {
                penaltyEntity.Penalty -= 10;
                
                // If penalty reaches 0, remove the entity
                if (penaltyEntity.Penalty == 0)
                {
                    _context.WhistledownPenalties.Remove(penaltyEntity);
                }
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
        }
    }

    // Game score methods
    public async Task<List<GameScore>> GetAllGameScoresAsync()
    {
        var entities = await _context.GameScores.ToListAsync();
        
        // Group by GameName and convert to GameScore model
        var gameScores = entities
            .GroupBy(e => e.GameName)
            .Select(g => new GameScore
            {
                GameName = g.Key,
                FamilyScores = g.ToDictionary(e => e.FamilyId, e => e.Score)
            })
            .Where(gs => gs.GameName != "Total")
            .ToList();

        // Calculate totals and penalties
        if (gameScores.Any())
        {
            var familyIds = gameScores.First().FamilyScores.Keys.ToList();
            var totalScores = new Dictionary<string, int>();
            var penaltyScores = new Dictionary<string, int>();

            // Calculate subtotal (without penalties)
            foreach (var familyId in familyIds)
            {
                var subtotal = gameScores.Sum(gs => gs.FamilyScores.ContainsKey(familyId) ? gs.FamilyScores[familyId] : 0);
                totalScores[familyId] = subtotal;
                penaltyScores[familyId] = 0;
            }

            // Get and apply penalties
            var penalties = await _context.WhistledownPenalties.ToListAsync();
            foreach (var penalty in penalties)
            {
                if (penaltyScores.ContainsKey(penalty.FamilyId))
                {
                    penaltyScores[penalty.FamilyId] = -penalty.Penalty; // Negative value for display
                    totalScores[penalty.FamilyId] -= penalty.Penalty;
                }
            }

            // Add Pénalités Whistledown row (only if there are penalties)
            if (penalties.Any(p => p.Penalty != 0))
            {
                gameScores.Add(new GameScore
                {
                    GameName = "Pénalités Whistledown",
                    FamilyScores = penaltyScores
                });
            }

            // Add Total row
            gameScores.Add(new GameScore
            {
                GameName = "Total",
                FamilyScores = totalScores
            });

            // Update family points in database
            foreach (var familyId in familyIds)
            {
                var family = await _context.Families.FindAsync(familyId);
                if (family != null && totalScores.ContainsKey(familyId))
                {
                    family.Points = totalScores[familyId];
                }
            }

            // Calculate and update ranks
            var rankedFamilies = await _context.Families.ToListAsync();
            var sortedFamilies = rankedFamilies.OrderByDescending(f => f.Points).ToList();
            for (int i = 0; i < sortedFamilies.Count; i++)
            {
                sortedFamilies[i].Rank = i + 1;
            }

            await _context.SaveChangesAsync();
        }

        return gameScores;
    }

    public async Task UpdateGameScoreAsync(string gameName, string familyId, int points)
    {
        var scoreEntity = await _context.GameScores
            .FirstOrDefaultAsync(g => g.GameName == gameName && g.FamilyId == familyId);

        if (scoreEntity != null)
        {
            scoreEntity.Score = points;
            await _context.SaveChangesAsync();
        }
        else
        {
            // Create new score if doesn't exist
            var newScore = new GameScoreEntity
            {
                GameName = gameName,
                FamilyId = familyId,
                Score = points
            };
            _context.GameScores.Add(newScore);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateGameScoreAsync(GameScore gameScore)
    {
        foreach (var familyScore in gameScore.FamilyScores)
        {
            await UpdateGameScoreAsync(gameScore.GameName, familyScore.Key, familyScore.Value);
        }
    }

    public async Task CreateGameScoreAsync(GameScore gameScore)
    {
        foreach (var familyScore in gameScore.FamilyScores)
        {
            var scoreEntity = new GameScoreEntity
            {
                GameName = gameScore.GameName,
                FamilyId = familyScore.Key,
                Score = familyScore.Value
            };
            _context.GameScores.Add(scoreEntity);
        }
        await _context.SaveChangesAsync();
    }

    public async Task DeleteGameScoreAsync(string gameName)
    {
        var scoreEntities = await _context.GameScores
            .Where(g => g.GameName == gameName)
            .ToListAsync();

        if (scoreEntities.Any())
        {
            _context.GameScores.RemoveRange(scoreEntities);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Dictionary<string, int>> GetPenaltiesAsync()
    {
        var penalties = await _context.WhistledownPenalties.ToListAsync();
        return penalties.ToDictionary(p => p.FamilyId, p => p.Penalty);
    }

    public async Task UpdateWhistledownPenaltyAsync(string familyId, int penalty)
    {
        var penaltyEntity = await _context.WhistledownPenalties.FindAsync(familyId);
        if (penaltyEntity != null)
        {
            penaltyEntity.Penalty = penalty;
            await _context.SaveChangesAsync();
        }
        else
        {
            var newPenalty = new WhistledownPenalty
            {
                FamilyId = familyId,
                Penalty = penalty
            };
            _context.WhistledownPenalties.Add(newPenalty);
            await _context.SaveChangesAsync();
        }
    }

    // Auth methods
    public async Task<bool> ValidateAdminAsync(string username, string password)
    {
        var admin = await _context.AdminCredentials
            .FirstOrDefaultAsync(a => a.Username == username);
        
        if (admin == null)
            return false;

        // Verify the password using BCrypt
        return BCrypt.Net.BCrypt.Verify(password, admin.Password);
    }
}
