namespace BridgertonGame.Shared.Models;

public class Quiz
{
    public int Id { get; set; }
    public int QuestionNumber { get; set; }
    public string Question { get; set; } = string.Empty;
    public string OptionA { get; set; } = string.Empty;
    public string OptionB { get; set; } = string.Empty;
    public string OptionC { get; set; } = string.Empty;
    public string OptionD { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty; // A, B, C, ou D
}

public class QuizAnswer
{
    public int Id { get; set; }
    public string PlayerId { get; set; } = string.Empty;
    public int QuestionNumber { get; set; }
    public string SelectedAnswer { get; set; } = string.Empty; // A, B, C, ou D
    public bool IsCorrect { get; set; }
    public DateTime AnsweredAt { get; set; }
}

public class QuizState
{
    public int Id { get; set; }
    public bool IsEnabled { get; set; }
    public int CurrentQuestionNumber { get; set; }
}

public class QuizStatistics
{
    public int QuestionNumber { get; set; }
    public Dictionary<string, int> AnswerCounts { get; set; } = new();
    public int TotalAnswers { get; set; }
    public string CorrectAnswer { get; set; } = string.Empty;
    public List<FamilyQuizResponse> FamilyResponses { get; set; } = new();
}

public class FamilyQuizResponse
{
    public string FamilyId { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public string PlayerName { get; set; } = string.Empty;
    public string SelectedAnswer { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public DateTime AnsweredAt { get; set; }
}
