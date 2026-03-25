using Microsoft.EntityFrameworkCore;
using BridgertonGame.Shared.Models;
using BridgertonGame.Server.Data.Entities;

namespace BridgertonGame.Server.Data;

public class BridgertonDbContext : DbContext
{
    public BridgertonDbContext(DbContextOptions<BridgertonDbContext> options)
        : base(options)
    {
    }

    public DbSet<Player> Players { get; set; }
    public DbSet<Family> Families { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<GameScoreEntity> GameScores { get; set; }
    public DbSet<PublicationCooldown> PublicationCooldowns { get; set; }
    public DbSet<WhistledownPenalty> WhistledownPenalties { get; set; }
    public DbSet<AdminCredential> AdminCredentials { get; set; }
    public DbSet<Vote> Votes { get; set; }
    public DbSet<VoteResult> VoteResults { get; set; }
    public DbSet<QuizEntity> Quizzes { get; set; }
    public DbSet<QuizAnswerEntity> QuizAnswers { get; set; }
    public DbSet<QuizStateEntity> QuizStates { get; set; }
    public DbSet<Entities.ChatMessage> ChatMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure primary keys
        modelBuilder.Entity<Player>().HasKey(p => p.Id);
        modelBuilder.Entity<Family>().HasKey(f => f.Id);
        modelBuilder.Entity<Article>().HasKey(a => a.Id);
        modelBuilder.Entity<GameScoreEntity>().HasKey(g => g.Id);
        modelBuilder.Entity<PublicationCooldown>().HasKey(p => p.FamilyId);
        modelBuilder.Entity<WhistledownPenalty>().HasKey(w => w.FamilyId);
        modelBuilder.Entity<AdminCredential>().HasKey(a => a.Id);
        modelBuilder.Entity<Vote>().HasKey(v => v.Id);
        modelBuilder.Entity<VoteResult>().HasKey(vr => vr.Id);
        modelBuilder.Entity<QuizEntity>().HasKey(q => q.Id);
        modelBuilder.Entity<QuizAnswerEntity>().HasKey(qa => qa.Id);
        modelBuilder.Entity<QuizStateEntity>().HasKey(qs => qs.Id);
        modelBuilder.Entity<Entities.ChatMessage>().HasKey(cm => cm.Id);

        // Configure unique indexes for Quiz
        modelBuilder.Entity<QuizEntity>()
            .HasIndex(q => q.QuestionNumber)
            .IsUnique();

        // Configure composite index for QuizAnswers
        modelBuilder.Entity<QuizAnswerEntity>()
            .HasIndex(qa => new { qa.PlayerId, qa.QuestionNumber })
            .IsUnique();

        // Seed initial data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Players
        modelBuilder.Entity<Player>().HasData(
            new Player { Id = "h1", Code = "CELIA2024", Name = "Célia Hastings", Title = "DUCHESSE", ImageUrl = "images/AdminAvatar.png", Role = "Lady Whistledown", FamilyId = "hastings", IsLadyWhistledown = true },
            new Player { Id = "h2", Code = "FANNY2024", Name = "Fanny Hastings", Title = "DUCHESSE", ImageUrl = "images/AdminAvatar.png", Role = "Invitée", FamilyId = "hastings", IsLadyWhistledown = false },
            new Player { Id = "h3", Code = "HUGO2024", Name = "Hugo Hastings", Title = "DUC", ImageUrl = "images/AdminAvatar.png", Role = "Invité", FamilyId = "hastings", IsLadyWhistledown = false },
            
            new Player { Id = "b1", Code = "DAPHNE2024", Name = "Daphné Bridgerton", Title = "DUCHESSE", ImageUrl = "images/AdminAvatar.png", Role = "Lady Whistledown", FamilyId = "bridgerton", IsLadyWhistledown = true },
            new Player { Id = "b2", Code = "SIMON2024", Name = "Simon Bridgerton", Title = "DUC", ImageUrl = "images/AdminAvatar.png", Role = "Invité", FamilyId = "bridgerton", IsLadyWhistledown = false },
            new Player { Id = "b3", Code = "ELOISE2024", Name = "Eloïse Bridgerton", Title = "LADY", ImageUrl = "images/AdminAvatar.png", Role = "Invitée", FamilyId = "bridgerton", IsLadyWhistledown = false },
            
            new Player { Id = "f1", Code = "PENELOPE2024", Name = "Penelope Featherington", Title = "LADY", ImageUrl = "images/AdminAvatar.png", Role = "Lady Whistledown", FamilyId = "featherington", IsLadyWhistledown = true },
            new Player { Id = "f2", Code = "PORTIA2024", Name = "Portia Featherington", Title = "LADY", ImageUrl = "images/AdminAvatar.png", Role = "Invitée", FamilyId = "featherington", IsLadyWhistledown = false },
            
            new Player { Id = "d1", Code = "AGATHA2024", Name = "Agatha Danbury", Title = "LADY", ImageUrl = "images/AdminAvatar.png", Role = "Maîtresse de soirée", FamilyId = "danbury", IsLadyWhistledown = false },
            new Player { Id = "d2", Code = "WILL2024", Name = "Will Danbury", Title = "LORD", ImageUrl = "images/AdminAvatar.png", Role = "Invité", FamilyId = "danbury", IsLadyWhistledown = false },
            
            new Player { Id = "s1", Code = "KATE2024", Name = "Kate Sharma", Title = "LADY", ImageUrl = "images/AdminAvatar.png", Role = "Lady Whistledown", FamilyId = "sharma", IsLadyWhistledown = true },
            new Player { Id = "s2", Code = "EDWINA2024", Name = "Edwina Sharma", Title = "LADY", ImageUrl = "images/AdminAvatar.png", Role = "Invitée", FamilyId = "sharma", IsLadyWhistledown = false }
        );

        // Seed Families
        modelBuilder.Entity<Family>().HasData(
            new Family { Id = "hastings", Name = "Hastings", Points = 230, Rank = 1, VotingEnabled = false, Revealed = false, LadyWhistledownId = "h1" },
            new Family { Id = "bridgerton", Name = "Bridgerton", Points = 210, Rank = 2, VotingEnabled = false, Revealed = false, LadyWhistledownId = "b1" },
            new Family { Id = "featherington", Name = "Featherington", Points = 180, Rank = 3, VotingEnabled = false, Revealed = false, LadyWhistledownId = "f1" },
            new Family { Id = "danbury", Name = "Danbury", Points = 150, Rank = 4, VotingEnabled = false, Revealed = false, LadyWhistledownId = null },
            new Family { Id = "sharma", Name = "Sharma", Points = 120, Rank = 5, VotingEnabled = false, Revealed = false, LadyWhistledownId = "s1" }
        );

        // Seed Articles
        var baseTime = DateTime.UtcNow.AddHours(-10);
        modelBuilder.Entity<Article>().HasData(
            new Article { Id = "1", Title = "Chers amis lecteurs,", Content = "La notation que la personne va écrire", FamilyId = "hastings", FamilyName = "Hastings", PublishedAt = baseTime },
            new Article { Id = "2", Title = "Chers amis lecteurs,", Content = "Un événement des plus intéressants s'est déroulé lors du dernier bal...", FamilyId = "bridgerton", FamilyName = "Bridgerton", PublishedAt = baseTime.AddHours(2) },
            new Article { Id = "3", Title = "Chers amis lecteurs,", Content = "Les rumeurs circulent à propos d'une certaine famille...", FamilyId = "featherington", FamilyName = "Featherington", PublishedAt = baseTime.AddHours(4) },
            new Article { Id = "4", Title = "Chers amis lecteurs,", Content = "Les secrets de la haute société ne me sont pas étrangers...", FamilyId = "hastings", FamilyName = "Hastings", PublishedAt = baseTime.AddHours(6) },
            new Article { Id = "5", Title = "Chers amis lecteurs,", Content = "Une nouvelle intrigue secoue les salons londoniens...", FamilyId = "danbury", FamilyName = "Danbury", PublishedAt = baseTime.AddHours(8) }
        );

        // Seed Game Scores
        modelBuilder.Entity<GameScoreEntity>().HasData(
            // Total scores
            new GameScoreEntity { Id = 1, GameName = "Total", FamilyId = "hastings", Score = 230 },
            new GameScoreEntity { Id = 2, GameName = "Total", FamilyId = "bridgerton", Score = 230 },
            new GameScoreEntity { Id = 3, GameName = "Total", FamilyId = "featherington", Score = 230 },
            new GameScoreEntity { Id = 4, GameName = "Total", FamilyId = "danbury", Score = 230 },
            new GameScoreEntity { Id = 5, GameName = "Total", FamilyId = "sharma", Score = 230 },
            
            // Jeu 1
            new GameScoreEntity { Id = 6, GameName = "Jeu 1", FamilyId = "hastings", Score = 230 },
            new GameScoreEntity { Id = 7, GameName = "Jeu 1", FamilyId = "bridgerton", Score = 230 },
            new GameScoreEntity { Id = 8, GameName = "Jeu 1", FamilyId = "featherington", Score = 230 },
            new GameScoreEntity { Id = 9, GameName = "Jeu 1", FamilyId = "danbury", Score = 230 },
            new GameScoreEntity { Id = 10, GameName = "Jeu 1", FamilyId = "sharma", Score = 230 },
            
            // Jeu 2
            new GameScoreEntity { Id = 11, GameName = "Jeu 2", FamilyId = "hastings", Score = 230 },
            new GameScoreEntity { Id = 12, GameName = "Jeu 2", FamilyId = "bridgerton", Score = 230 },
            new GameScoreEntity { Id = 13, GameName = "Jeu 2", FamilyId = "featherington", Score = 230 },
            new GameScoreEntity { Id = 14, GameName = "Jeu 2", FamilyId = "danbury", Score = 230 },
            new GameScoreEntity { Id = 15, GameName = "Jeu 2", FamilyId = "sharma", Score = 230 },
            
            // Jeu 3
            new GameScoreEntity { Id = 16, GameName = "Jeu 3", FamilyId = "hastings", Score = 230 },
            new GameScoreEntity { Id = 17, GameName = "Jeu 3", FamilyId = "bridgerton", Score = 230 },
            new GameScoreEntity { Id = 18, GameName = "Jeu 3", FamilyId = "featherington", Score = 230 },
            new GameScoreEntity { Id = 19, GameName = "Jeu 3", FamilyId = "danbury", Score = 230 },
            new GameScoreEntity { Id = 20, GameName = "Jeu 3", FamilyId = "sharma", Score = 230 },
            
            // Whistledown penalties as scores
            new GameScoreEntity { Id = 21, GameName = "Whistledown", FamilyId = "hastings", Score = 0 },
            new GameScoreEntity { Id = 22, GameName = "Whistledown", FamilyId = "bridgerton", Score = -10 },
            new GameScoreEntity { Id = 23, GameName = "Whistledown", FamilyId = "featherington", Score = 0 },
            new GameScoreEntity { Id = 24, GameName = "Whistledown", FamilyId = "danbury", Score = -10 },
            new GameScoreEntity { Id = 25, GameName = "Whistledown", FamilyId = "sharma", Score = 0 }
        );

        // Seed Whistledown Penalties
        modelBuilder.Entity<WhistledownPenalty>().HasData(
            new WhistledownPenalty { FamilyId = "hastings", Penalty = 0 },
            new WhistledownPenalty { FamilyId = "bridgerton", Penalty = -10 },
            new WhistledownPenalty { FamilyId = "featherington", Penalty = 0 },
            new WhistledownPenalty { FamilyId = "danbury", Penalty = -10 },
            new WhistledownPenalty { FamilyId = "sharma", Penalty = 0 }
        );

        // Seed Admin Credentials
        // BCrypt hash of "bridgerton2024" with work factor 11
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("bridgerton2024", 11);
        modelBuilder.Entity<AdminCredential>().HasData(
            new AdminCredential { Id = 1, Username = "admin", Password = hashedPassword }
        );
    }
}
