using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;
using BridgertonGame.Client.Services;
using BridgertonGame.Shared.Models;
using BridgertonGame.Shared.DTOs;

namespace BridgertonGame.Client.Pages;

public partial class MonEspace : IAsyncDisposable
{
    [Inject] private ApiService ApiService { get; set; } = default!;
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private HttpClient Http { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    private Player? currentPlayer;
    private Family? currentFamily;
    private List<Player> familyMembers = new();
    private List<Player> otherLadyWhistledowns = new();
    private List<Family> allFamilies = new();
    private string playerCode = "";
    private string errorMessage = "";
    private string successMessage = "";
    private bool showRole = false;
    private string selectedMemberId = "";
    private string? existingVoteForName = null;
    private bool hasVoted = false;
    private string articleContent = "";
    private int charCount = 0;
    private TimeSpan? timeUntilNext;
    private System.Threading.Timer? cooldownTimer;
    private bool showModal = false;
    private string selectedImage = "";
    private int playerPoints = 0;

    // Team Whistledown stats
    private int teamWhistledownPoints = 0;
    private int teamWhistledownRank = 0;

    // Quiz
    private QuizState? quizState;
    private BridgertonGame.Shared.Models.Quiz? currentQuestion;
    private QuizAnswer? currentPlayerAnswer;
    private string selectedQuizAnswer = "";

    // SignalR
    private HubConnection? hubConnection;

    protected override async Task OnInitializedAsync()
    {
        currentPlayer = await AuthService.GetCurrentPlayerAsync();
        if (currentPlayer != null)
        {
            var freshPlayer = await ApiService.GetPlayerByCodeAsync(currentPlayer.Code);
            if (freshPlayer != null)
            {
                currentPlayer = freshPlayer;
                await AuthService.SetCurrentPlayerAsync(freshPlayer);
            }
            
            await LoadPlayerData();
        }

        await InitializeSignalR();
    }

    private async Task InitializeSignalR()
    {
        try
        {
            var hubUrl = Navigation.ToAbsoluteUri("/notificationHub");
            hubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect()
                .Build();

            hubConnection.On<int, bool>("QuizUpdated", async (questionNumber, isEnabled) =>
            {
                Console.WriteLine($"Quiz updated: Question {questionNumber}, Enabled: {isEnabled}");
                await OnQuizUpdated(questionNumber, isEnabled);
            });

            await hubConnection.StartAsync();
            Console.WriteLine("SignalR connected for MonEspace");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to SignalR: {ex.Message}");
        }
    }

    private async Task OnQuizUpdated(int questionNumber, bool isEnabled)
    {
        try
        {
            await LoadQuizData();
            
            if (currentQuestion?.QuestionNumber != questionNumber)
            {
                selectedQuizAnswer = "";
            }
            
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling quiz update: {ex.Message}");
        }
    }

    private async Task LoadPlayerData()
    {
        if (currentPlayer == null) return;

        try
        {
            if (currentPlayer.Role == "Maîtresse de maison")
            {
                return;
            }
            
            currentFamily = await ApiService.GetFamilyByIdAsync(currentPlayer.FamilyId);
            
            if (currentFamily == null)
            {
                errorMessage = "Impossible de charger les données de votre famille. Veuillez vérifier que le serveur est démarré.";
                return;
            }
            
            familyMembers = await ApiService.GetPlayersByFamilyAsync(currentPlayer.FamilyId);
            allFamilies = await ApiService.GetAllFamiliesAsync();
            
            if (currentPlayer.IsLadyWhistledown)
            {
                await LoadOtherLadyWhistledowns();
            }
            
            await CheckExistingVote();
            await LoadQuizData();
            
            if (currentPlayer.IsLadyWhistledown)
            {
                var penalties = await ApiService.GetPenaltiesAsync();
                playerPoints = penalties.ContainsKey(currentPlayer.FamilyId) ? penalties[currentPlayer.FamilyId] : 0;
                
                teamWhistledownPoints = await ApiService.GetLadyWhistledownTeamPointsAsync();
                teamWhistledownRank = await CalculateTeamWhistledownRank();
                
                await CheckCooldown();
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Erreur de connexion au serveur: {ex.Message}";
            Console.WriteLine($"Erreur LoadPlayerData: {ex}");
        }
    }

    private async Task LoadOtherLadyWhistledowns()
    {
        if (currentPlayer == null) return;

        try
        {
            var allPlayers = await ApiService.GetAllPlayersAsync();
            
            otherLadyWhistledowns = allPlayers
                .Where(p => p.IsLadyWhistledown && p.FamilyId != currentPlayer.FamilyId)
                .ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur LoadOtherLadyWhistledowns: {ex.Message}");
            otherLadyWhistledowns = new();
        }
    }

    private async Task LoadQuizData()
    {
        if (currentPlayer == null) return;

        try
        {
            quizState = await Http.GetFromJsonAsync<QuizState>("api/quiz/state");
            
            if (quizState?.IsEnabled == true && quizState.CurrentQuestionNumber > 0)
            {
                currentQuestion = await Http.GetFromJsonAsync<BridgertonGame.Shared.Models.Quiz>(
                    $"api/quiz/questions/{quizState.CurrentQuestionNumber}");
                
                try
                {
                    currentPlayerAnswer = await Http.GetFromJsonAsync<QuizAnswer>(
                        $"api/quiz/player-answer/{currentPlayer.Id}/{quizState.CurrentQuestionNumber}");
                }
                catch
                {
                    currentPlayerAnswer = null;
                }
            }
            else
            {
                currentQuestion = null;
                currentPlayerAnswer = null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur LoadQuizData: {ex.Message}");
        }
    }

    private async Task CheckExistingVote()
    {
        if (currentPlayer == null || currentFamily == null) return;

        try
        {
            var voteResults = await ApiService.GetVoteResultsAsync(currentFamily.Id);
            var playerVote = voteResults?.Votes.FirstOrDefault(v => v.VoterId == currentPlayer.Id);
            
            if (playerVote != null)
            {
                hasVoted = true;
                existingVoteForName = playerVote.VotedForName;
            }
            else
            {
                hasVoted = false;
                existingVoteForName = null;
            }
        }
        catch
        {
            hasVoted = false;
            existingVoteForName = null;
        }
    }

    private async Task ValidateCode()
    {
        errorMessage = "";
        successMessage = "";
        
        if (string.IsNullOrWhiteSpace(playerCode))
        {
            errorMessage = "Veuillez entrer un code";
            return;
        }

        var player = await ApiService.GetPlayerByCodeAsync(playerCode);
        if (player != null)
        {
            currentPlayer = player;
            await AuthService.SetCurrentPlayerAsync(player);
            await LoadPlayerData();
        }
        else
        {
            errorMessage = "Code invalide. Veuillez réessayer.";
        }
    }

    private async Task HandleKeyPress(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await ValidateCode();
        }
    }

    private void ToggleRoleVisibility()
    {
        showRole = !showRole;
    }

    private string GetRankText(int rank) => rank switch
    {
        1 => "1er",
        2 => "2ème",
        3 => "3ème",
        4 => "4ème",
        5 => "5ème",
        _ => $"{rank}ème"
    };

    private async Task ConfirmVote()
    {
        if (string.IsNullOrEmpty(selectedMemberId) || currentFamily == null || currentPlayer == null) 
            return;

        try
        {
            errorMessage = "";
            successMessage = "";
            
            var request = new VoteRequest 
            { 
                FamilyId = currentFamily.Id, 
                VoterId = currentPlayer.Id,
                PlayerId = selectedMemberId 
            };
            
            await Http.PostAsJsonAsync($"api/families/{currentFamily.Id}/vote", request);
            await CheckExistingVote();
            
            successMessage = "Votre vote a été enregistré !";
            _ = ClearMessagesAfterDelay();
        }
        catch
        {
            errorMessage = "Erreur lors de l'enregistrement du vote.";
            _ = ClearMessagesAfterDelay();
        }
    }

    private void UpdateCharCount(ChangeEventArgs e)
    {
        articleContent = e.Value?.ToString() ?? "";
        charCount = articleContent.Length;
    }

    private bool CanPublish()
    {
        return !string.IsNullOrWhiteSpace(articleContent) && 
               (!timeUntilNext.HasValue || timeUntilNext.Value <= TimeSpan.Zero);
    }

    private async Task PublishArticle()
    {
        if (!CanPublish() || currentFamily == null) return;

        try
        {
            errorMessage = "";
            successMessage = "";
            
            var request = new PublishArticleRequest
            {
                Title = "Chers amis lecteurs,",
                Content = articleContent,
                FamilyId = currentFamily.Id
            };

            var response = await Http.PostAsJsonAsync("api/articles", request);
            if (response.IsSuccessStatusCode)
            {
                articleContent = "";
                charCount = 0;
                await CheckCooldown();
                
                if (currentPlayer?.IsLadyWhistledown == true)
                {
                    var penalties = await ApiService.GetPenaltiesAsync();
                    playerPoints = penalties.ContainsKey(currentPlayer.FamilyId) 
                        ? penalties[currentPlayer.FamilyId] 
                        : 0;
                }
                
                successMessage = "Votre article a été publié !";
                _ = ClearMessagesAfterDelay();
            }
        }
        catch
        {
            errorMessage = "Erreur lors de la publication.";
            _ = ClearMessagesAfterDelay();
        }
    }

    private async Task CheckCooldown()
    {
        if (currentFamily == null) return;

        try
        {
            var response = await Http.GetFromJsonAsync<System.Text.Json.JsonElement>(
                $"api/articles/can-publish/{currentFamily.Id}");
            
            var canPublish = response.GetProperty("canPublish").GetBoolean();
            
            if (!canPublish && response.TryGetProperty("timeRemaining", out var timeRemainingProp))
            {
                var timeRemainingStr = timeRemainingProp.GetString();
                if (!string.IsNullOrEmpty(timeRemainingStr))
                {
                    timeUntilNext = TimeSpan.Parse(timeRemainingStr);
                    StartCooldownTimer();
                }
            }
            else
            {
                timeUntilNext = null;
            }
        }
        catch { }
    }

    private void StartCooldownTimer()
    {
        cooldownTimer?.Dispose();
        cooldownTimer = new System.Threading.Timer(async _ =>
        {
            if (timeUntilNext.HasValue && timeUntilNext.Value > TimeSpan.Zero)
            {
                timeUntilNext = timeUntilNext.Value.Subtract(TimeSpan.FromSeconds(1));
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                cooldownTimer?.Dispose();
                timeUntilNext = null;
                await InvokeAsync(StateHasChanged);
            }
        }, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
    }

    private string FormatTimeRemaining(TimeSpan time)
    {
        return $"{time.Minutes:D2}:{time.Seconds:D2}";
    }

    private void ShowImageModal(string imageUrl)
    {
        selectedImage = imageUrl;
        showModal = true;
    }

    private void CloseImageModal()
    {
        showModal = false;
        selectedImage = "";
    }

    private async Task Logout()
    {
        await AuthService.LogoutAsync();
        currentPlayer = null;
        currentFamily = null;
        showRole = false;
        selectedMemberId = "";
        articleContent = "";
        playerCode = "";
        errorMessage = "";
        successMessage = "";
        cooldownTimer?.Dispose();
    }

    private async Task ClearMessagesAfterDelay()
    {
        await Task.Delay(5000);
        errorMessage = "";
        successMessage = "";
        StateHasChanged();
    }

    private async Task SubmitQuizAnswer()
    {
        if (string.IsNullOrEmpty(selectedQuizAnswer) || currentPlayer == null || currentQuestion == null) 
            return;

        try
        {
            errorMessage = "";
            successMessage = "";
            
            var request = new QuizAnswerRequest
            {
                PlayerId = currentPlayer.Id,
                QuestionNumber = currentQuestion.QuestionNumber,
                SelectedAnswer = selectedQuizAnswer
            };

            var response = await Http.PostAsJsonAsync("api/quiz/answer", request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<QuizAnswerResponse>();
                successMessage = result?.Message ?? "Réponse enregistrée !";
                
                await LoadQuizData();
                selectedQuizAnswer = "";
                
                _ = ClearMessagesAfterDelay();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                errorMessage = $"Erreur: {errorContent}";
                _ = ClearMessagesAfterDelay();
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Erreur lors de l'envoi de la réponse: {ex.Message}";
            _ = ClearMessagesAfterDelay();
        }
    }

    private void GoToQuiz()
    {
        Navigation.NavigateTo("/quiz");
    }

    private async Task<int> CalculateTeamWhistledownRank()
    {
        try
        {
            var families = await ApiService.GetAllFamiliesAsync();
            var teamPoints = await ApiService.GetLadyWhistledownTeamPointsAsync();
            
            var rank = 1;
            foreach (var family in families)
            {
                if (family.Points > teamPoints)
                {
                    rank++;
                }
            }
            
            return rank;
        }
        catch
        {
            return 0;
        }
    }

    public async ValueTask DisposeAsync()
    {
        cooldownTimer?.Dispose();
        
        if (hubConnection is not null)
        {
            await hubConnection.DisposeAsync();
        }
    }
}
