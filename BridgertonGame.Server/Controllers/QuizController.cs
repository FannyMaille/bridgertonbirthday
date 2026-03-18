using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using BridgertonGame.Server.Data;
using BridgertonGame.Server.Data.Entities;
using BridgertonGame.Server.Hubs;
using BridgertonGame.Shared.Models;
using BridgertonGame.Shared.DTOs;

namespace BridgertonGame.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizController : ControllerBase
{
    private readonly BridgertonDbContext _context;
    private readonly IHubContext<NotificationHub> _hubContext;

    public QuizController(BridgertonDbContext context, IHubContext<NotificationHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    // GET: api/quiz/state
    [HttpGet("state")]
    public async Task<ActionResult<QuizState>> GetQuizState()
    {
        var state = await _context.QuizStates.FirstOrDefaultAsync();
        if (state == null)
        {
            // Créer un état par défaut
            state = new QuizStateEntity { IsEnabled = false, CurrentQuestionNumber = 0 };
            _context.QuizStates.Add(state);
            await _context.SaveChangesAsync();
        }
        return Ok(state.ToModel());
    }

    // PUT: api/quiz/state
    [HttpPut("state")]
    public async Task<IActionResult> UpdateQuizState(QuizState state)
    {
        var existing = await _context.QuizStates.FirstOrDefaultAsync();
        if (existing == null)
        {
            var newState = QuizStateEntity.FromModel(state);
            _context.QuizStates.Add(newState);
        }
        else
        {
            existing.IsEnabled = state.IsEnabled;
            existing.CurrentQuestionNumber = state.CurrentQuestionNumber;
        }
        await _context.SaveChangesAsync();
        
        // Notifier tous les clients du changement
        await _hubContext.Clients.All.SendAsync("QuizUpdated", state.CurrentQuestionNumber, state.IsEnabled);
        
        return Ok();
    }

    // GET: api/quiz/questions
    [HttpGet("questions")]
    public async Task<ActionResult<List<Quiz>>> GetAllQuestions()
    {
        var questions = await _context.Quizzes
            .OrderBy(q => q.QuestionNumber)
            .Select(q => q.ToModel())
            .ToListAsync();
        return Ok(questions);
    }

    // GET: api/quiz/questions/{questionNumber}
    [HttpGet("questions/{questionNumber}")]
    public async Task<ActionResult<Quiz>> GetQuestion(int questionNumber)
    {
        var question = await _context.Quizzes
            .FirstOrDefaultAsync(q => q.QuestionNumber == questionNumber);
        
        if (question == null)
            return NotFound();

        return Ok(question.ToModel());
    }

    // GET: api/quiz/current
    [HttpGet("current")]
    public async Task<ActionResult<Quiz>> GetCurrentQuestion()
    {
        var state = await _context.QuizStates.FirstOrDefaultAsync();
        if (state == null || !state.IsEnabled || state.CurrentQuestionNumber == 0)
            return NotFound(new { message = "Aucune question active" });

        var question = await _context.Quizzes
            .FirstOrDefaultAsync(q => q.QuestionNumber == state.CurrentQuestionNumber);
        
        if (question == null)
            return NotFound(new { message = "Question introuvable" });

        return Ok(question.ToModel());
    }

    // POST: api/quiz/questions
    [HttpPost("questions")]
    public async Task<ActionResult<Quiz>> CreateQuestion(Quiz quiz)
    {
        // Vérifier si le numéro de question existe déjà
        var existing = await _context.Quizzes
            .FirstOrDefaultAsync(q => q.QuestionNumber == quiz.QuestionNumber);
        
        if (existing != null)
            return BadRequest(new { message = "Ce numéro de question existe déjà" });

        var entity = QuizEntity.FromModel(quiz);
        _context.Quizzes.Add(entity);
        await _context.SaveChangesAsync();
        
        quiz.Id = entity.Id;
        return CreatedAtAction(nameof(GetQuestion), new { questionNumber = quiz.QuestionNumber }, quiz);
    }

    // PUT: api/quiz/questions/{id}
    [HttpPut("questions/{id}")]
    public async Task<IActionResult> UpdateQuestion(int id, Quiz quiz)
    {
        var existing = await _context.Quizzes.FindAsync(id);
        if (existing == null)
            return NotFound();

        // Vérifier si le nouveau numéro de question n'est pas déjà utilisé par une autre question
        if (existing.QuestionNumber != quiz.QuestionNumber)
        {
            var duplicate = await _context.Quizzes
                .FirstOrDefaultAsync(q => q.QuestionNumber == quiz.QuestionNumber && q.Id != id);
            if (duplicate != null)
                return BadRequest(new { message = "Ce numéro de question existe déjà" });
        }

        existing.QuestionNumber = quiz.QuestionNumber;
        existing.Question = quiz.Question;
        existing.OptionA = quiz.OptionA;
        existing.OptionB = quiz.OptionB;
        existing.OptionC = quiz.OptionC;
        existing.OptionD = quiz.OptionD;
        existing.CorrectAnswer = quiz.CorrectAnswer;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/quiz/questions/{id}
    [HttpDelete("questions/{id}")]
    public async Task<IActionResult> DeleteQuestion(int id)
    {
        var question = await _context.Quizzes.FindAsync(id);
        if (question == null)
            return NotFound();

        // Supprimer également les réponses associées
        var answers = await _context.QuizAnswers
            .Where(a => a.QuestionNumber == question.QuestionNumber)
            .ToListAsync();
        _context.QuizAnswers.RemoveRange(answers);

        _context.Quizzes.Remove(question);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // POST: api/quiz/answer
    [HttpPost("answer")]
    public async Task<ActionResult<QuizAnswerResponse>> SubmitAnswer(QuizAnswerRequest request)
    {
        // Vérifier que le quiz est actif
        var state = await _context.QuizStates.FirstOrDefaultAsync();
        if (state == null || !state.IsEnabled)
            return BadRequest(new { message = "Le quiz n'est pas actif" });

        // Vérifier que la question existe et correspond à la question actuelle
        var question = await _context.Quizzes
            .FirstOrDefaultAsync(q => q.QuestionNumber == request.QuestionNumber);
        
        if (question == null)
            return NotFound(new { message = "Question introuvable" });

        if (question.QuestionNumber != state.CurrentQuestionNumber)
            return BadRequest(new { message = "Cette question n'est pas la question active" });

        // Vérifier si le joueur a déjà répondu à cette question
        var existingAnswer = await _context.QuizAnswers
            .FirstOrDefaultAsync(a => a.PlayerId == request.PlayerId && a.QuestionNumber == request.QuestionNumber);
        
        if (existingAnswer != null)
            return BadRequest(new { message = "Vous avez déjà répondu à cette question" });

        // Enregistrer la réponse
        var isCorrect = request.SelectedAnswer.Equals(question.CorrectAnswer, StringComparison.OrdinalIgnoreCase);
        var answer = new QuizAnswerEntity
        {
            PlayerId = request.PlayerId,
            QuestionNumber = request.QuestionNumber,
            SelectedAnswer = request.SelectedAnswer,
            IsCorrect = isCorrect,
            AnsweredAt = DateTime.UtcNow
        };

        _context.QuizAnswers.Add(answer);
        await _context.SaveChangesAsync();

        return Ok(new QuizAnswerResponse
        {
            IsCorrect = isCorrect,
            Message = isCorrect ? "Bonne réponse !" : "Mauvaise réponse"
        });
    }

    // GET: api/quiz/player-answer/{playerId}/{questionNumber}
    [HttpGet("player-answer/{playerId}/{questionNumber}")]
    public async Task<ActionResult<QuizAnswer>> GetPlayerAnswer(string playerId, int questionNumber)
    {
        var answer = await _context.QuizAnswers
            .FirstOrDefaultAsync(a => a.PlayerId == playerId && a.QuestionNumber == questionNumber);
        
        if (answer == null)
            return NotFound();

        return Ok(answer.ToModel());
    }

    // GET: api/quiz/statistics/{questionNumber}
    [HttpGet("statistics/{questionNumber}")]
    public async Task<ActionResult<QuizStatistics>> GetQuestionStatistics(int questionNumber)
    {
        var question = await _context.Quizzes
            .FirstOrDefaultAsync(q => q.QuestionNumber == questionNumber);
        
        if (question == null)
            return NotFound();

        var answers = await _context.QuizAnswers
            .Where(a => a.QuestionNumber == questionNumber)
            .ToListAsync();

        // Récupérer les informations des joueurs et familles
        var familyResponses = new List<FamilyQuizResponse>();
        foreach (var answer in answers)
        {
            var player = await _context.Players.FindAsync(answer.PlayerId);
            if (player != null)
            {
                var family = await _context.Families.FindAsync(player.FamilyId);
                familyResponses.Add(new FamilyQuizResponse
                {
                    FamilyId = player.FamilyId,
                    FamilyName = family?.Name ?? "Inconnue",
                    PlayerName = player.Name,
                    SelectedAnswer = answer.SelectedAnswer,
                    IsCorrect = answer.IsCorrect,
                    AnsweredAt = answer.AnsweredAt
                });
            }
        }

        var stats = new QuizStatistics
        {
            QuestionNumber = questionNumber,
            CorrectAnswer = question.CorrectAnswer,
            TotalAnswers = answers.Count,
            AnswerCounts = new Dictionary<string, int>
            {
                ["A"] = answers.Count(a => a.SelectedAnswer == "A"),
                ["B"] = answers.Count(a => a.SelectedAnswer == "B"),
                ["C"] = answers.Count(a => a.SelectedAnswer == "C"),
                ["D"] = answers.Count(a => a.SelectedAnswer == "D")
            },
            FamilyResponses = familyResponses.OrderBy(f => f.FamilyName).ToList()
        };

        return Ok(stats);
    }

    // GET: api/quiz/all-statistics
    [HttpGet("all-statistics")]
    public async Task<ActionResult<List<QuizStatistics>>> GetAllStatistics()
    {
        var questions = await _context.Quizzes
            .OrderBy(q => q.QuestionNumber)
            .ToListAsync();

        var allStats = new List<QuizStatistics>();

        foreach (var question in questions)
        {
            var answers = await _context.QuizAnswers
                .Where(a => a.QuestionNumber == question.QuestionNumber)
                .ToListAsync();

            // Récupérer les informations des joueurs et familles
            var familyResponses = new List<FamilyQuizResponse>();
            foreach (var answer in answers)
            {
                var player = await _context.Players.FindAsync(answer.PlayerId);
                if (player != null)
                {
                    var family = await _context.Families.FindAsync(player.FamilyId);
                    familyResponses.Add(new FamilyQuizResponse
                    {
                        FamilyId = player.FamilyId,
                        FamilyName = family?.Name ?? "Inconnue",
                        PlayerName = player.Name,
                        SelectedAnswer = answer.SelectedAnswer,
                        IsCorrect = answer.IsCorrect,
                        AnsweredAt = answer.AnsweredAt
                    });
                }
            }

            var stats = new QuizStatistics
            {
                QuestionNumber = question.QuestionNumber,
                CorrectAnswer = question.CorrectAnswer,
                TotalAnswers = answers.Count,
                AnswerCounts = new Dictionary<string, int>
                {
                    ["A"] = answers.Count(a => a.SelectedAnswer == "A"),
                    ["B"] = answers.Count(a => a.SelectedAnswer == "B"),
                    ["C"] = answers.Count(a => a.SelectedAnswer == "C"),
                    ["D"] = answers.Count(a => a.SelectedAnswer == "D")
                },
                FamilyResponses = familyResponses.OrderBy(f => f.FamilyName).ToList()
            };

            allStats.Add(stats);
        }

        return Ok(allStats);
    }
}
