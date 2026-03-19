using BridgertonGame.Shared.Models;

namespace BridgertonGame.Shared.DTOs;

public class LoginRequest
{
    public string Code { get; set; } = string.Empty;
}

public class LoginResponse
{
    public bool Success { get; set; }
    public string? PlayerId { get; set; }
    public string? ErrorMessage { get; set; }
}

public class AdminLoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class AdminLoginResponse
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? ErrorMessage { get; set; }
}

public class PublishArticleRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string FamilyId { get; set; } = string.Empty;
}

public class PublishArticleResponse
{
    public bool Success { get; set; }
    public Article? Article { get; set; }
    public string? ErrorMessage { get; set; }
    public TimeSpan? TimeUntilNext { get; set; }
}

public class VoteRequest
{
    public string FamilyId { get; set; } = string.Empty;
    public string VoterId { get; set; } = string.Empty;
    public string PlayerId { get; set; } = string.Empty; // The player being voted for
}

public class SetWhistledownRequest
{
    public string? PlayerId { get; set; }
}

public class QuizAnswerRequest
{
    public string PlayerId { get; set; } = string.Empty;
    public int QuestionNumber { get; set; }
    public string SelectedAnswer { get; set; } = string.Empty; // A, B, C, or D
}

public class QuizAnswerResponse
{
    public bool IsCorrect { get; set; }
    public string Message { get; set; } = string.Empty;
}
