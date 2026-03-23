using BridgertonGame.Shared.Models;

namespace BridgertonGame.Server.Data.Entities;

public class QuizEntity
{
    public int Id { get; set; }
    public int QuestionNumber { get; set; }
    public string Question { get; set; } = string.Empty;
    public string OptionA { get; set; } = string.Empty;
    public string OptionB { get; set; } = string.Empty;
    public string OptionC { get; set; } = string.Empty;
    public string OptionD { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;

    public Quiz ToModel()
    {
        return new Quiz
        {
            Id = Id,
            QuestionNumber = QuestionNumber,
            Question = Question,
            OptionA = OptionA,
            OptionB = OptionB,
            OptionC = OptionC,
            OptionD = OptionD,
            CorrectAnswer = CorrectAnswer
        };
    }

    public static QuizEntity FromModel(Quiz quiz)
    {
        return new QuizEntity
        {
            Id = quiz.Id,
            QuestionNumber = quiz.QuestionNumber,
            Question = quiz.Question,
            OptionA = quiz.OptionA,
            OptionB = quiz.OptionB,
            OptionC = quiz.OptionC,
            OptionD = quiz.OptionD,
            CorrectAnswer = quiz.CorrectAnswer
        };
    }
}

public class QuizAnswerEntity
{
    public int Id { get; set; }
    public string PlayerId { get; set; } = string.Empty;
    public int QuestionNumber { get; set; }
    public string SelectedAnswer { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public DateTime AnsweredAt { get; set; }

    public QuizAnswer ToModel()
    {
        return new QuizAnswer
        {
            Id = Id,
            PlayerId = PlayerId,
            QuestionNumber = QuestionNumber,
            SelectedAnswer = SelectedAnswer,
            IsCorrect = IsCorrect,
            AnsweredAt = AnsweredAt
        };
    }

    public static QuizAnswerEntity FromModel(QuizAnswer answer)
    {
        return new QuizAnswerEntity
        {
            Id = answer.Id,
            PlayerId = answer.PlayerId,
            QuestionNumber = answer.QuestionNumber,
            SelectedAnswer = answer.SelectedAnswer,
            IsCorrect = answer.IsCorrect,
            AnsweredAt = answer.AnsweredAt
        };
    }
}

public class QuizStateEntity
{
    public int Id { get; set; }
    public bool IsEnabled { get; set; }
    public int CurrentQuestionNumber { get; set; }

    public QuizState ToModel()
    {
        return new QuizState
        {
            Id = Id,
            IsEnabled = IsEnabled,
            CurrentQuestionNumber = CurrentQuestionNumber
        };
    }

    public static QuizStateEntity FromModel(QuizState state)
    {
        return new QuizStateEntity
        {
            Id = state.Id,
            IsEnabled = state.IsEnabled,
            CurrentQuestionNumber = state.CurrentQuestionNumber
        };
    }
}
