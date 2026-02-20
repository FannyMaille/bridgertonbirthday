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
            await _context.SaveChangesAsync();
        }
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

        await _context.SaveChangesAsync();
        return article;
    }

    public async Task DeleteArticleAsync(string articleId)
    {
        var article = await _context.Articles.FindAsync(articleId);
        if (article != null)
        {
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
            .ToList();

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
            .FirstOrDefaultAsync(a => a.Username == username && a.Password == password);
        return admin != null;
    }
}
