using BridgertonGame.Shared.Models;

namespace BridgertonGame.Server.Services;

public class GameDataService
{
    private readonly List<Player> _players;
    private readonly List<Family> _families;
    private readonly List<Article> _articles;
    private readonly List<GameScore> _gameScores;
    private readonly Dictionary<string, DateTime> _lastPublicationTimes;
    private readonly Dictionary<string, int> _whistledownPenalties;

    public GameDataService()
    {
        _players = InitializePlayers();
        _families = InitializeFamilies();
        _articles = InitializeArticles();
        _gameScores = InitializeGameScores();
        _lastPublicationTimes = new Dictionary<string, DateTime>();
        _whistledownPenalties = new Dictionary<string, int>
        {
            { "hastings", 0 },
            { "bridgerton", -10 },
            { "featherington", 0 },
            { "danbury", -10 },
            { "sharma", 0 }
        };
    }

    private List<Player> InitializePlayers()
    {
        return new List<Player>
        {
            new() { Id = "h1", Code = "CELIA2024", Name = "Célia Hastings", Title = "DUCHESSE", ImageUrl = "images/characters/celia-hastings.png", Role = "Lady Whistledown", FamilyId = "hastings", IsLadyWhistledown = true },
            new() { Id = "h2", Code = "FANNY2024", Name = "Fanny Hastings", Title = "DUCHESSE", ImageUrl = "images/characters/fanny-hastings.png", Role = "Invitée", FamilyId = "hastings", IsLadyWhistledown = false },
            new() { Id = "h3", Code = "HUGO2024", Name = "Hugo Hastings", Title = "DUC", ImageUrl = "images/characters/hugo-hastings.png", Role = "Invité", FamilyId = "hastings", IsLadyWhistledown = false },
            
            new() { Id = "b1", Code = "DAPHNE2024", Name = "Daphné Bridgerton", Title = "DUCHESSE", ImageUrl = "images/characters/daphne-bridgerton.png", Role = "Lady Whistledown", FamilyId = "bridgerton", IsLadyWhistledown = true },
            new() { Id = "b2", Code = "SIMON2024", Name = "Simon Bridgerton", Title = "DUC", ImageUrl = "images/characters/simon-bridgerton.png", Role = "Invité", FamilyId = "bridgerton", IsLadyWhistledown = false },
            new() { Id = "b3", Code = "ELOISE2024", Name = "Eloïse Bridgerton", Title = "LADY", ImageUrl = "images/characters/eloise-bridgerton.png", Role = "Invitée", FamilyId = "bridgerton", IsLadyWhistledown = false },
            
            new() { Id = "f1", Code = "PENELOPE2024", Name = "Penelope Featherington", Title = "LADY", ImageUrl = "images/characters/penelope-featherington.png", Role = "Lady Whistledown", FamilyId = "featherington", IsLadyWhistledown = true },
            new() { Id = "f2", Code = "PORTIA2024", Name = "Portia Featherington", Title = "LADY", ImageUrl = "images/characters/portia-featherington.png", Role = "Invitée", FamilyId = "featherington", IsLadyWhistledown = false },
            
            new() { Id = "d1", Code = "AGATHA2024", Name = "Fanny Maille", Title = "LADY", ImageUrl = "images/characters/fanny-hastings.png", Role = "Maîtresse de soirée", FamilyId = "danbury", IsLadyWhistledown = false },
            new() { Id = "d2", Code = "WILL2024", Name = "Will Danbury", Title = "LORD", ImageUrl = "images/characters/will-danbury.png", Role = "Invité", FamilyId = "danbury", IsLadyWhistledown = false },
            
            new() { Id = "s1", Code = "KATE2024", Name = "Kate Sharma", Title = "LADY", ImageUrl = "images/characters/kate-sharma.png", Role = "Lady Whistledown", FamilyId = "sharma", IsLadyWhistledown = true },
            new() { Id = "s2", Code = "EDWINA2024", Name = "Edwina Sharma", Title = "LADY", ImageUrl = "images/characters/edwina-sharma.png", Role = "Invitée", FamilyId = "sharma", IsLadyWhistledown = false }
        };
    }

    private List<Family> InitializeFamilies()
    {
        return new List<Family>
        {
            new() { Id = "hastings", Name = "Hastings", Points = 230, Rank = 1, VotingEnabled = false, Revealed = false, LadyWhistledownId = "h1" },
            new() { Id = "bridgerton", Name = "Bridgerton", Points = 210, Rank = 2, VotingEnabled = false, Revealed = false, LadyWhistledownId = "b1" },
            new() { Id = "featherington", Name = "Featherington", Points = 180, Rank = 3, VotingEnabled = false, Revealed = false, LadyWhistledownId = "f1" },
            new() { Id = "danbury", Name = "Danbury", Points = 150, Rank = 4, VotingEnabled = false, Revealed = false, LadyWhistledownId = null },
            new() { Id = "sharma", Name = "Sharma", Points = 120, Rank = 5, VotingEnabled = false, Revealed = false, LadyWhistledownId = "s1" }
        };
    }

    private List<Article> InitializeArticles()
    {
        var now = DateTime.UtcNow;
        return new List<Article>
        {
            new() { Id = "1", Title = "Chers amis lecteurs,", Content = "La notation que la personne va écrire", FamilyId = "hastings", FamilyName = "Hastings", PublishedAt = now.AddHours(-2) },
            new() { Id = "2", Title = "Chers amis lecteurs,", Content = "Un événement des plus intéressants s'est déroulé lors du dernier bal...", FamilyId = "bridgerton", FamilyName = "Bridgerton", PublishedAt = now.AddHours(-4) },
            new() { Id = "3", Title = "Chers amis lecteurs,", Content = "Les rumeurs circulent à propos d'une certaine famille...", FamilyId = "featherington", FamilyName = "Featherington", PublishedAt = now.AddHours(-6) },
            new() { Id = "4", Title = "Chers amis lecteurs,", Content = "Les secrets de la haute société ne me sont pas étrangers...", FamilyId = "hastings", FamilyName = "Hastings", PublishedAt = now.AddHours(-8) },
            new() { Id = "5", Title = "Chers amis lecteurs,", Content = "Une nouvelle intrigue secoue les salons londoniens...", FamilyId = "danbury", FamilyName = "Danbury", PublishedAt = now.AddHours(-10) }
        };
    }

    private List<GameScore> InitializeGameScores()
    {
        var scores = new Dictionary<string, int>
        {
            { "hastings", 230 },
            { "bridgerton", 230 },
            { "featherington", 230 },
            { "danbury", 230 },
            { "sharma", 230 }
        };

        return new List<GameScore>
        {
            new() { GameName = "Total", FamilyScores = new Dictionary<string, int>(scores) },
            new() { GameName = "Jeu 3", FamilyScores = new Dictionary<string, int>(scores) },
            new() { GameName = "Whistledown", FamilyScores = new Dictionary<string, int> { { "hastings", 0 }, { "bridgerton", -10 }, { "featherington", 0 }, { "danbury", -10 }, { "sharma", 0 } } },
            new() { GameName = "Jeu 2", FamilyScores = new Dictionary<string, int>(scores) },
            new() { GameName = "Whistledown", FamilyScores = new Dictionary<string, int> { { "hastings", 0 }, { "bridgerton", 0 }, { "featherington", -10 }, { "danbury", 0 }, { "sharma", 0 } } },
            new() { GameName = "Jeu 1", FamilyScores = new Dictionary<string, int>(scores) }
        };
    }

    public Player? GetPlayerByCode(string code) => 
        _players.FirstOrDefault(p => p.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

    public Player? GetPlayerById(string id) => 
        _players.FirstOrDefault(p => p.Id == id);

    public List<Player> GetAllPlayers() => _players;

    public List<Player> GetPlayersByFamily(string familyId) => 
        _players.Where(p => p.FamilyId == familyId).ToList();

    public List<Family> GetAllFamilies() => _families;

    public Family? GetFamilyById(string id) => 
        _families.FirstOrDefault(f => f.Id == id);

    public void UpdateFamilyPoints(string familyId, int points)
    {
        var family = GetFamilyById(familyId);
        if (family != null)
        {
            family.Points = points;
        }
    }

    public void SetLadyWhistledown(string familyId, string playerId)
    {
        var family = GetFamilyById(familyId);
        if (family != null)
        {
            family.LadyWhistledownId = playerId;
        }
    }

    public void ToggleVoting(string familyId, bool enabled)
    {
        var family = GetFamilyById(familyId);
        if (family != null)
        {
            family.VotingEnabled = enabled;
        }
    }

    public void RevealLadyWhistledown(string familyId)
    {
        var family = GetFamilyById(familyId);
        if (family != null)
        {
            family.Revealed = true;
        }
    }

    public List<Article> GetAllArticles() => 
        _articles.OrderByDescending(a => a.PublishedAt).ToList();

    public List<Article> GetArticlesByFamily(string familyId) => 
        _articles.Where(a => a.FamilyId == familyId)
                 .OrderByDescending(a => a.PublishedAt)
                 .ToList();

    public bool CanPublish(string familyId)
    {
        if (!_lastPublicationTimes.TryGetValue(familyId, out var lastTime))
            return true;

        var diffMinutes = (DateTime.UtcNow - lastTime).TotalMinutes;
        return diffMinutes >= 30;
    }

    public TimeSpan? GetTimeUntilNextPublication(string familyId)
    {
        if (!_lastPublicationTimes.TryGetValue(familyId, out var lastTime))
            return null;

        var nextTime = lastTime.AddMinutes(30);
        var remaining = nextTime - DateTime.UtcNow;

        return remaining > TimeSpan.Zero ? remaining : null;
    }

    public Article PublishArticle(string title, string content, string familyId, string familyName)
    {
        var article = new Article
        {
            Id = DateTime.UtcNow.Ticks.ToString(),
            Title = title,
            Content = content,
            FamilyId = familyId,
            FamilyName = familyName,
            PublishedAt = DateTime.UtcNow
        };

        _articles.Add(article);
        _lastPublicationTimes[familyId] = DateTime.UtcNow;

        return article;
    }

    public void DeleteArticle(string articleId)
    {
        var article = _articles.FirstOrDefault(a => a.Id == articleId);
        if (article != null)
        {
            _articles.Remove(article);
        }
    }

    public List<GameScore> GetAllGameScores() => _gameScores;

    public void UpdateGameScore(string gameName, string familyId, int points)
    {
        var game = _gameScores.FirstOrDefault(g => g.GameName == gameName);
        if (game != null)
        {
            game.FamilyScores[familyId] = points;
        }
    }

    public void UpdateWhistledownPenalty(string familyId, int penalty)
    {
        _whistledownPenalties[familyId] = penalty;
    }

    public Dictionary<string, int> GetWhistledownPenalties() => _whistledownPenalties;
}
