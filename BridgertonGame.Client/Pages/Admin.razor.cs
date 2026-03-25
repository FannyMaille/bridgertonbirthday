using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using BridgertonGame.Client.Services;
using BridgertonGame.Shared.Models;
using BridgertonGame.Shared.DTOs;
using System.Net.Http.Json;

namespace BridgertonGame.Client.Pages
{
    public partial class Admin
    {
        [Inject] private ApiService ApiService { get; set; } = default!;
        [Inject] private AuthService AuthService { get; set; } = default!;
        [Inject] private HttpClient Http { get; set; } = default!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

        private bool isAdmin = false;
        private string username = "";
        private string password = "";
        private string errorMessage = "";
        private string currentTab = "scores";
        private string searchQuery = "";
        private bool isMobileMenuOpen = false;

        private List<Family>? families;
        private List<GameScore>? gameScores;
        private List<Article>? articles;
        private List<Player>? players;
        private Dictionary<string, int> penalties = new();
        private List<FamilyVoteResult>? voteResults;

        private string? editingGameId = null;
        private string editingGameName = "";
        private Player? editingPlayer = null;
        private bool showEditModal = false;
        private bool isCreatingPlayer = false;

        private Family? editingFamily = null;
        private bool showEditFamilyModal = false;
        private bool isCreatingFamily = false;
        private string familyValidationError = "";

        // Quiz management
        private QuizState? quizState;
        private List<Quiz>? quizQuestions;
        private List<QuizStatistics>? quizStatistics;
        private Quiz? editingQuestion = null;
        private bool showEditQuestionModal = false;
        private bool isCreatingQuestion = false;
        private string questionValidationError = "";
        private int currentQuestionNumber = 0;
        private List<FamilyQuizSummary>? familyQuizSummary;

        private HashSet<string> visibleCodes = new();
        private HashSet<string> visibleRoles = new();
        private HashSet<string> visibleWhistledowns = new();

        // Timer dialog
        private bool showSetTimerDialog = false;
        private string timerFamilyId = "";
        private string timerFamilyName = "";
        private int timerMinutes = 15;

        // Chat management
        private List<ChatMessage>? chatMessages;
        private int chatMessageCount = 0;

        protected override async Task OnInitializedAsync()
        {
            isAdmin = await AuthService.IsAdminAuthenticatedAsync();
            if (isAdmin)
            {
                await LoadData();
            }
        }

        private async Task LoadData()
        {
            families = await ApiService.GetAllFamiliesAsync();
            gameScores = await ApiService.GetAllGameScoresAsync();
            articles = await ApiService.GetAllArticlesAsync();
            players = await ApiService.GetAllPlayersAsync();
            penalties = await ApiService.GetPenaltiesAsync();
            voteResults = await ApiService.GetAllVoteResultsAsync();

            // Load quiz data
            await LoadQuizData();
            
            // Load chat data
            await LoadChatData();
        }

        private async Task LoadQuizData()
        {
            try
            {
                quizState = await Http.GetFromJsonAsync<QuizState>("api/quiz/state");
                currentQuestionNumber = quizState?.CurrentQuestionNumber ?? 0;
                quizQuestions = await Http.GetFromJsonAsync<List<Quiz>>("api/quiz/questions");
                quizStatistics = await Http.GetFromJsonAsync<List<QuizStatistics>>("api/quiz/all-statistics");
                familyQuizSummary = await Http.GetFromJsonAsync<List<FamilyQuizSummary>>("api/quiz/family-summary");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur LoadQuizData: {ex.Message}");
            }
        }

        private async Task ValidateLogin()
        {
            var request = new AdminLoginRequest { Username = username, Password = password };
            try
            {
                var response = await Http.PostAsJsonAsync("api/auth/admin", request);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AdminLoginResponse>();
                    if (result?.Success == true)
                    {
                        isAdmin = true;
                        await AuthService.SetAdminAuthenticatedAsync(true);
                        await LoadData();
                        errorMessage = "";
                    }
                    else
                    {
                        errorMessage = result?.ErrorMessage ?? "Erreur de connexion";
                    }
                }
                else
                {
                    errorMessage = "Login ou mot de passe incorrect";
                }
            }
            catch
            {
                errorMessage = "Erreur de connexion";
            }
        }

        private async Task HandleKeyPress(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await ValidateLogin();
            }
        }

        private void SetTab(string tab)
        {
            currentTab = tab;
            isMobileMenuOpen = false;
        }

        private void SelectTab(string tab)
        {
            SetTab(tab);
        }

        private void ToggleMobileMenu()
        {
            isMobileMenuOpen = !isMobileMenuOpen;
        }

        private void CloseMobileMenu()
        {
            isMobileMenuOpen = false;
        }

        private async Task UpdateScore(string gameName, string familyId, string? value)
        {
            if (int.TryParse(value, out int points) && gameScores != null)
            {
                var game = gameScores.FirstOrDefault(g => g.GameName == gameName);
                if (game != null)
                {
                    game.FamilyScores[familyId] = points;
                    await Http.PutAsJsonAsync("api/gamescores", game);
                }
            }
        }

        private async Task UpdatePenalty(string familyId, string? value)
        {
            if (int.TryParse(value, out int penalty))
            {
                penalties[familyId] = penalty;
                await Http.PutAsJsonAsync($"api/gamescores/penalties/{familyId}", penalty);
            }
        }

        private void StartEditGameName(string gameName)
        {
            editingGameId = gameName;
            editingGameName = gameName;
        }

        private void CancelEditGameName()
        {
            editingGameId = null;
            editingGameName = "";
        }

        private async Task SaveGameName(string oldGameName)
        {
            if (string.IsNullOrWhiteSpace(editingGameName) || gameScores == null)
            {
                CancelEditGameName();
                return;
            }

            var game = gameScores.FirstOrDefault(g => g.GameName == oldGameName);
            if (game != null && oldGameName != editingGameName)
            {
                // Delete old game
                await Http.DeleteAsync($"api/gamescores/{Uri.EscapeDataString(oldGameName)}");

                // Create new game with new name
                game.GameName = editingGameName;
                await Http.PostAsJsonAsync("api/gamescores", game);

                await LoadData();
            }

            CancelEditGameName();
        }

        private async Task AddNewGame()
        {
            if (families == null || gameScores == null) return;

            var newGameName = $"Nouveau Jeu {gameScores.Count(g => g.GameName != "Total" && g.GameName != "Pénalités Whistledown") + 1}";

            var newGame = new GameScore
            {
                GameName = newGameName,
                FamilyScores = new Dictionary<string, int>()
            };

            foreach (var family in families)
            {
                newGame.FamilyScores[family.Id] = 0;
            }

            await Http.PostAsJsonAsync("api/gamescores", newGame);
            await LoadData();
        }

        private async Task DeleteGame(string gameName)
        {
            if (gameScores == null) return;

            var confirmed = await JSRuntime.InvokeAsync<bool>("confirm", $"Êtes-vous sûr de vouloir supprimer le jeu '{gameName}' ?\n\nCette action est irréversible.");

            if (confirmed)
            {
                await Http.DeleteAsync($"api/gamescores/{Uri.EscapeDataString(gameName)}");
                await LoadData();
            }
        }

        private async Task DeleteArticle(string articleId)
        {
            await Http.DeleteAsync($"api/articles/{articleId}");
            await LoadData();
        }

        private async Task ToggleVoting(string familyId, bool enabled)
        {
            await Http.PostAsJsonAsync($"api/families/{familyId}/toggle-voting", enabled);
            await LoadData();
        }

        private async Task ToggleReveal(string familyId, bool revealed)
        {
            await Http.PostAsJsonAsync($"api/families/{familyId}/toggle-reveal", revealed);
            await LoadData();
        }

        private async Task RevealWhistledown(string familyId)
        {
            await Http.PostAsync($"api/families/{familyId}/reveal", null);
            await LoadData();
        }

        private async Task RevealAll()
        {
            await Http.PostAsync("api/families/reveal-all", null);
            await LoadData();
        }

        private async Task DeleteVote(string familyId, string voterId, string voterName)
        {
            var confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
                $"Êtes-vous sûr de vouloir supprimer le vote de '{voterName}' ?\n\nSi la famille est révélée, les points seront recalculés automatiquement.");

            if (confirmed)
            {
                var success = await ApiService.DeleteVoteAsync(familyId, voterId);
                if (success)
                {
                    await LoadData();
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("alert", "Erreur lors de la suppression du vote.");
                }
            }
        }

        private async Task Logout()
        {
            await AuthService.SetAdminAuthenticatedAsync(false);
            isAdmin = false;
            username = "";
            password = "";
            errorMessage = "";
        }

        private void EditUser(Player player)
        {
            editingPlayer = new Player
            {
                Id = player.Id,
                Code = player.Code,
                Name = player.Name,
                Title = player.Title,
                ImageUrl = player.ImageUrl,
                Role = player.Role,
                FamilyId = player.FamilyId,
                IsLadyWhistledown = player.IsLadyWhistledown
            };

            // S'assurer que le rôle et IsLadyWhistledown sont synchronisés
            if (editingPlayer.IsLadyWhistledown && editingPlayer.Role != "Lady Whistledown")
            {
                editingPlayer.Role = "Lady Whistledown";
            }
            else if (!editingPlayer.IsLadyWhistledown && editingPlayer.Role == "Lady Whistledown")
            {
                editingPlayer.IsLadyWhistledown = true;
            }

            isCreatingPlayer = false;
            showEditModal = true;
        }

        private async Task ToggleLadyWhistledown(Player player)
        {
            var confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
                player.IsLadyWhistledown
                    ? $"Retirer '{player.Name}' en tant que Lady Whistledown de sa famille ?"
                    : $"Définir '{player.Name}' comme Lady Whistledown de sa famille ?\n\nCela remplacera l'actuelle Lady Whistledown si elle existe.");

            if (confirmed)
            {
                var family = families?.FirstOrDefault(f => f.Id == player.FamilyId);
                if (family != null)
                {
                    if (player.IsLadyWhistledown)
                    {
                        // Retirer Lady Whistledown
                        await Http.PostAsJsonAsync($"api/families/{family.Id}/set-whistledown", new { PlayerId = (string?)null });
                    }
                    else
                    {
                        // Définir comme Lady Whistledown
                        await Http.PostAsJsonAsync($"api/families/{family.Id}/set-whistledown", new { PlayerId = player.Id });
                    }
                    await LoadData();
                }
            }
        }

        private void CloseEditModal()
        {
            editingPlayer = null;
            showEditModal = false;
        }

        private async Task SaveUser()
        {
            if (editingPlayer == null) return;

            try
            {
                if (isCreatingPlayer)
                {
                    // Création d'un nouvel utilisateur
                    await Http.PostAsJsonAsync("api/players", editingPlayer);
                }
                else
                {
                    // Modification d'un utilisateur existant
                    await Http.PutAsJsonAsync($"api/players/{editingPlayer.Id}", editingPlayer);
                }
                await LoadData();
                CloseEditModal();
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", $"Erreur lors de la sauvegarde : {ex.Message}");
            }
        }

        private void ToggleCodeVisibility(string playerId)
        {
            if (visibleCodes.Contains(playerId))
            {
                visibleCodes.Remove(playerId);
            }
            else
            {
                visibleCodes.Add(playerId);
            }
        }

        private void ToggleRoleVisibility(string playerId)
        {
            if (visibleRoles.Contains(playerId))
            {
                visibleRoles.Remove(playerId);
            }
            else
            {
                visibleRoles.Add(playerId);
            }
        }

        private void ToggleWhistledownVisibility(string familyId)
        {
            if (visibleWhistledowns.Contains(familyId))
            {
                visibleWhistledowns.Remove(familyId);
            }
            else
            {
                visibleWhistledowns.Add(familyId);
            }
        }

        private void AddNewPlayer()
        {
            editingPlayer = new Player
            {
                Id = Guid.NewGuid().ToString(),
                Name = "",
                Title = "",
                Code = "",
                Role = "",
                ImageUrl = "",
                FamilyId = families?.FirstOrDefault()?.Id,
                IsLadyWhistledown = false
            };
            isCreatingPlayer = true;
            showEditModal = true;
        }

        private async Task DeletePlayer(Player player)
        {
            var confirmed = await JSRuntime.InvokeAsync<bool>("confirm", $"Êtes-vous sûr de vouloir supprimer l'utilisateur '{player.Name}' ?\n\nCette action est irréversible.");

            if (confirmed)
            {
                await Http.DeleteAsync($"api/players/{player.Id}");
                await LoadData();
            }
        }

        private bool IsPlayerValid()
        {
            if (editingPlayer == null) return false;

            // Pour une Maîtresse de maison, la famille n'est pas obligatoire
            bool familyValid = editingPlayer.Role == "Maîtresse de maison" ||
                              !string.IsNullOrWhiteSpace(editingPlayer.FamilyId);

            return !string.IsNullOrWhiteSpace(editingPlayer.Name) &&
                   !string.IsNullOrWhiteSpace(editingPlayer.Title) &&
                   !string.IsNullOrWhiteSpace(editingPlayer.Code) &&
                   !string.IsNullOrWhiteSpace(editingPlayer.Role) &&
                   !string.IsNullOrWhiteSpace(editingPlayer.ImageUrl) &&
                   familyValid;
        }

        private void OnRoleChanged()
        {
            if (editingPlayer == null) return;

            // Mettre à jour IsLadyWhistledown automatiquement selon le rôle sélectionné
            editingPlayer.IsLadyWhistledown = editingPlayer.Role == "Lady Whistledown";

            // Si c'est une Maîtresse de maison, retirer la famille
            if (editingPlayer.Role == "Maîtresse de maison")
            {
                editingPlayer.FamilyId = null;
            }
        }

        // Family management methods
        private void AddNewFamily()
        {
            editingFamily = new Family
            {
                Id = Guid.NewGuid().ToString(),
                Name = "",
                Points = 0,
                Rank = (families?.Count ?? 0) + 1,
                VotingEnabled = false,
                Revealed = false,
                LadyWhistledownId = null
            };
            isCreatingFamily = true;
            showEditFamilyModal = true;
        }

        private void EditFamily(Family family)
        {
            editingFamily = new Family
            {
                Id = family.Id,
                Name = family.Name,
                Points = family.Points,
                Rank = family.Rank,
                VotingEnabled = family.VotingEnabled,
                Revealed = family.Revealed,
                LadyWhistledownId = family.LadyWhistledownId
            };
            isCreatingFamily = false;
            showEditFamilyModal = true;
        }

        private void CloseEditFamilyModal()
        {
            editingFamily = null;
            showEditFamilyModal = false;
            isCreatingFamily = false;
            familyValidationError = "";
        }

        private async Task SaveFamily()
        {
            if (editingFamily == null) return;

            try
            {
                familyValidationError = "";

                if (isCreatingFamily)
                {
                    var response = await Http.PostAsJsonAsync("api/families", editingFamily);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        familyValidationError = $"Erreur serveur: {errorContent}";
                        await JSRuntime.InvokeVoidAsync("alert", $"Erreur lors de la création : {errorContent}");
                        return;
                    }
                }
                else
                {
                    var response = await Http.PutAsJsonAsync($"api/families/{editingFamily.Id}", editingFamily);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        familyValidationError = $"Erreur serveur: {errorContent}";
                        await JSRuntime.InvokeVoidAsync("alert", $"Erreur lors de la modification : {errorContent}");
                        return;
                    }
                }

                await LoadData();
                CloseEditFamilyModal();
            }
            catch (Exception ex)
            {
                familyValidationError = $"Exception: {ex.Message}";
                await JSRuntime.InvokeVoidAsync("alert", $"Erreur lors de la sauvegarde : {ex.Message}");
            }
        }

        private async Task DeleteFamily(Family family)
        {
            var familyPlayers = players?.Where(p => p.FamilyId == family.Id).ToList() ?? new List<Player>();

            if (familyPlayers.Any())
            {
                await JSRuntime.InvokeVoidAsync("alert", $"Impossible de supprimer la famille '{family.Name}' car elle contient {familyPlayers.Count} membre(s).\n\nVeuillez d'abord supprimer ou réassigner les membres.");
                return;
            }

            var confirmed = await JSRuntime.InvokeAsync<bool>("confirm", $"Êtes-vous sûr de vouloir supprimer la famille '{family.Name}' ?\n\nCette action est irréversible.");

            if (confirmed)
            {
                await Http.DeleteAsync($"api/families/{family.Id}");
                await LoadData();
            }
        }

        private bool IsFamilyValid()
        {
            if (editingFamily == null)
            {
                familyValidationError = "Aucune famille en cours d'édition";
                return false;
            }

            if (string.IsNullOrWhiteSpace(editingFamily.Name))
            {
                familyValidationError = "Le nom de la famille est requis";
                return false;
            }

            if (editingFamily.Rank < 1 || editingFamily.Rank > 6)
            {
                familyValidationError = "Le rang doit être entre 1 et 6";
                return false;
            }

            familyValidationError = "";
            return true;
        }

        private async Task ToggleQuizEnabled(bool enabled)
        {
            if (quizState == null) return;

            quizState.IsEnabled = enabled;
            await Http.PutAsJsonAsync("api/quiz/state", quizState);
            await LoadQuizData();
        }

        private async Task UpdateCurrentQuestion()
        {
            if (quizState == null) return;

            quizState.CurrentQuestionNumber = currentQuestionNumber;
            await Http.PutAsJsonAsync("api/quiz/state", quizState);
            await LoadQuizData();
        }

        private void AddNewQuestion()
        {
            var nextNumber = (quizQuestions?.Any() == true)
                ? quizQuestions.Max(q => q.QuestionNumber) + 1
                : 1;

            editingQuestion = new Quiz
            {
                QuestionNumber = nextNumber,
                Question = "",
                OptionA = "",
                OptionB = "",
                OptionC = "",
                OptionD = "",
                CorrectAnswer = ""
            };
            isCreatingQuestion = true;
            showEditQuestionModal = true;
        }

        private void EditQuestion(Quiz question)
        {
            editingQuestion = new Quiz
            {
                Id = question.Id,
                QuestionNumber = question.QuestionNumber,
                Question = question.Question,
                OptionA = question.OptionA,
                OptionB = question.OptionB,
                OptionC = question.OptionC,
                OptionD = question.OptionD,
                CorrectAnswer = question.CorrectAnswer
            };
            isCreatingQuestion = false;
            showEditQuestionModal = true;
        }

        private void CloseEditQuestionModal()
        {
            editingQuestion = null;
            showEditQuestionModal = false;
            isCreatingQuestion = false;
            questionValidationError = "";
        }

        private async Task SaveQuestion()
        {
            if (editingQuestion == null || !IsQuestionValid()) return;

            try
            {
                questionValidationError = "";

                if (isCreatingQuestion)
                {
                    var response = await Http.PostAsJsonAsync("api/quiz/questions", editingQuestion);
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        questionValidationError = $"Erreur: {errorContent}";
                        await JSRuntime.InvokeVoidAsync("alert", $"Erreur lors de la création : {errorContent}");
                        return;
                    }
                }
                else
                {
                    var response = await Http.PutAsJsonAsync($"api/quiz/questions/{editingQuestion.Id}", editingQuestion);
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        questionValidationError = $"Erreur: {errorContent}";
                        await JSRuntime.InvokeVoidAsync("alert", $"Erreur lors de la modification : {errorContent}");
                        return;
                    }
                }

                await LoadQuizData();
                CloseEditQuestionModal();
            }
            catch (Exception ex)
            {
                questionValidationError = $"Exception: {ex.Message}";
                await JSRuntime.InvokeVoidAsync("alert", $"Erreur lors de la sauvegarde : {ex.Message}");
            }
        }

        private async Task DeleteQuestion(int id)
        {
            var confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
                "Êtes-vous sûr de vouloir supprimer cette question ?\n\nToutes les réponses associées seront également supprimées.");

            if (confirmed)
            {
                await Http.DeleteAsync($"api/quiz/questions/{id}");
                await LoadQuizData();
            }
        }

        private bool IsQuestionValid()
        {
            if (editingQuestion == null)
            {
                questionValidationError = "Aucune question en cours d'édition";
                return false;
            }

            if (editingQuestion.QuestionNumber < 1)
            {
                questionValidationError = "Le numéro de question doit être supérieur à 0";
                return false;
            }

            if (string.IsNullOrWhiteSpace(editingQuestion.Question))
            {
                questionValidationError = "La question est requise";
                return false;
            }

            if (string.IsNullOrWhiteSpace(editingQuestion.OptionA) ||
                string.IsNullOrWhiteSpace(editingQuestion.OptionB) ||
                string.IsNullOrWhiteSpace(editingQuestion.OptionC) ||
                string.IsNullOrWhiteSpace(editingQuestion.OptionD))
            {
                questionValidationError = "Toutes les options (A, B, C, D) sont requises";
                return false;
            }

            if (string.IsNullOrWhiteSpace(editingQuestion.CorrectAnswer) ||
                !new[] { "A", "B", "C", "D" }.Contains(editingQuestion.CorrectAnswer))
            {
                questionValidationError = "Vous devez sélectionner la bonne réponse (A, B, C ou D)";
                return false;
            }

            questionValidationError = "";
            return true;
        }

        private async Task ResetAllQuizAnswers()
        {
            var totalAnswers = quizStatistics?.Sum(s => s.TotalAnswers) ?? 0;
            var confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
                $"⚠️ ATTENTION ⚠️\n\n" +
                $"Vous êtes sur le point de supprimer TOUTES les réponses au quiz !\n\n" +
                $"Total actuel : {totalAnswers} réponse(s) enregistrée(s)\n\n" +
                $"Cette action est IRRÉVERSIBLE.\n\n" +
                $"Voulez-vous vraiment continuer ?");

            if (confirmed)
            {
                try
                {
                    var response = await Http.DeleteAsync("api/quiz/answers/all");
                    if (response.IsSuccessStatusCode)
                    {
                        await LoadQuizData();
                        await JSRuntime.InvokeVoidAsync("alert", $"✅ Toutes les réponses ont été supprimées avec succès !\n\nLe quiz a été réinitialisé.");
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("alert", "❌ Erreur lors de la réinitialisation du quiz.");
                    }
                }
                catch (Exception ex)
                {
                    await JSRuntime.InvokeVoidAsync("alert", $"❌ Erreur : {ex.Message}");
                }
            }
        }

        private async Task DeleteIndividualAnswer(string playerId, int questionNumber, string playerName)
        {
            var confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
                $"Supprimer la réponse de '{playerName}' à la Question {questionNumber} ?\n\n" +
                $"Cette action est irréversible.");

            if (confirmed)
            {
                try
                {
                    var response = await Http.DeleteAsync($"api/quiz/answers/{playerId}/{questionNumber}");
                    if (response.IsSuccessStatusCode)
                    {
                        await LoadQuizData();
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("alert", "❌ Erreur lors de la suppression de la réponse.");
                    }
                }
                catch (Exception ex)
                {
                    await JSRuntime.InvokeVoidAsync("alert", $"❌ Erreur : {ex.Message}");
                }
            }
        }

        // Timer Lady Whistledown management methods
        private async Task ResetTimer(string familyId, string familyName)
        {
            var confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
                $"Réinitialiser le timer de Lady Whistledown pour la famille {familyName} ?\n\n" +
                $"Cette action permettra une nouvelle publication immédiate.");

            if (confirmed)
            {
                try
                {
                    var response = await Http.DeleteAsync($"api/families/{familyId}/timer");
                    if (response.IsSuccessStatusCode)
                    {
                        await LoadData();
                        await JSRuntime.InvokeVoidAsync("alert", $"✅ Timer réinitialisé pour la famille {familyName}");
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("alert", "❌ Erreur lors de la réinitialisation du timer.");
                    }
                }
                catch (Exception ex)
                {
                    await JSRuntime.InvokeVoidAsync("alert", $"❌ Erreur : {ex.Message}");
                }
            }
        }

        private void ShowSetTimerDialog(string familyId, string familyName)
        {
            timerFamilyId = familyId;
            timerFamilyName = familyName;
            timerMinutes = 15; // Valeur par défaut
            showSetTimerDialog = true;
        }

        private void CloseSetTimerDialog()
        {
            showSetTimerDialog = false;
            timerFamilyId = "";
            timerFamilyName = "";
            timerMinutes = 15;
        }

        private async Task ConfirmSetTimer()
        {
            if (timerMinutes < 0 || timerMinutes > 60)
            {
                await JSRuntime.InvokeVoidAsync("alert", "⚠️ Veuillez entrer une valeur entre 0 et 60 minutes.");
                return;
            }

            try
            {
                var response = await Http.PostAsJsonAsync($"api/families/{timerFamilyId}/timer/set", new { Minutes = timerMinutes });
                if (response.IsSuccessStatusCode)
                {
                    await LoadData();
                    await JSRuntime.InvokeVoidAsync("alert", $"✅ Timer défini à {timerMinutes} minutes pour la famille {timerFamilyName}");
                    CloseSetTimerDialog();
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("alert", "❌ Erreur lors de la définition du timer.");
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", $"❌ Erreur : {ex.Message}");
            }
        }

        private async Task SetTimerManually(string familyId, string familyName, int minutes)
        {
            try
            {
                var response = await Http.PostAsJsonAsync($"api/families/{familyId}/timer/set", new { Minutes = minutes });
                if (response.IsSuccessStatusCode)
                {
                    await LoadData();
                    await JSRuntime.InvokeVoidAsync("alert", $"✅ Timer défini à {minutes} minutes pour la famille {familyName}");
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("alert", "❌ Erreur lors de la définition du timer.");
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", $"❌ Erreur : {ex.Message}");
            }
        }

        private async Task<TimerStatus?> GetTimerStatus(string familyId)
        {
            try
            {
                var response = await Http.GetFromJsonAsync<TimerStatus>($"api/families/{familyId}/timer/status");
                return response;
            }
            catch
            {
                return null;
            }
        }

        private string FormatTimeRemaining(TimeSpan timeSpan)
        {
            if (timeSpan.TotalMinutes < 1)
                return "Moins d'une minute";
            else if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes} min";
            else
                return $"{timeSpan.Hours}h {timeSpan.Minutes}min";
        }

        // Chat management methods
        private async Task LoadChatData()
        {
            try
            {
                chatMessages = await Http.GetFromJsonAsync<List<ChatMessage>>("api/chat");
                chatMessageCount = await Http.GetFromJsonAsync<int>("api/chat/count");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur LoadChatData: {ex.Message}");
            }
        }

        private async Task ClearAllChatMessages()
        {
            var confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
                $"⚠️ ATTENTION ⚠️\n\n" +
                $"Vous êtes sur le point de supprimer TOUS les messages du chat !\n\n" +
                $"Total actuel : {chatMessageCount} message(s)\n\n" +
                $"Cette action est IRRÉVERSIBLE.\n\n" +
                $"Voulez-vous vraiment continuer ?");

            if (confirmed)
            {
                try
                {
                    var response = await Http.DeleteAsync("api/chat");
                    if (response.IsSuccessStatusCode)
                    {
                        await LoadChatData();
                        await JSRuntime.InvokeVoidAsync("alert", "✅ Tous les messages ont été supprimés avec succès !");
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("alert", "❌ Erreur lors de la suppression des messages.");
                    }
                }
                catch (Exception ex)
                {
                    await JSRuntime.InvokeVoidAsync("alert", $"❌ Erreur : {ex.Message}");
                }
            }
        }

        private string FormatChatTime(DateTime dateTime)
        {
            var localTime = dateTime.ToLocalTime();
            var now = DateTime.Now;

            if (localTime.Date == now.Date)
            {
                return localTime.ToString("HH:mm");
            }
            else if (localTime.Date == now.AddDays(-1).Date)
            {
                return $"Hier {localTime:HH:mm}";
            }
            else
            {
                return localTime.ToString("dd/MM HH:mm");
            }
        }
    }

    // DTO for timer status
    public class TimerStatus
    {
        public bool CanPublish { get; set; }
        public TimeSpan? TimeUntilNext { get; set; }
        public DateTime? LastPublicationTime { get; set; }
    }
}
