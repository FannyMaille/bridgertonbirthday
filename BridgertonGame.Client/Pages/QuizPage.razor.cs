using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;
using BridgertonGame.Client.Services;
using BridgertonGame.Shared.Models;
using BridgertonGame.Shared.DTOs;

namespace BridgertonGame.Client.Pages
{
    public partial class QuizPage : IAsyncDisposable
    {
        [Inject] private ApiService ApiService { get; set; } = default!;
        [Inject] private AuthService AuthService { get; set; } = default!;
        [Inject] private HttpClient Http { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private Player? currentPlayer;
        private QuizState? quizState;
        private Quiz? currentQuestion;
        private QuizAnswer? currentPlayerAnswer;
        private string selectedQuizAnswer = "";
        private string errorMessage = "";
        private string successMessage = "";
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
                
                await LoadQuizData();
            }

            // Initialiser SignalR
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

                // Écouter les mises à jour du quiz
                hubConnection.On<int, bool>("QuizUpdated", async (questionNumber, isEnabled) =>
                {
                    Console.WriteLine($"Quiz updated: Question {questionNumber}, Enabled: {isEnabled}");
                    await OnQuizUpdated(questionNumber, isEnabled);
                });

                await hubConnection.StartAsync();
                Console.WriteLine("SignalR connected for Quiz page");
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
                // Recharger les données du quiz
                await LoadQuizData();
                
                // Réinitialiser la sélection si c'est une nouvelle question
                if (currentQuestion?.QuestionNumber != questionNumber)
                {
                    selectedQuizAnswer = "";
                }
                
                // Mettre à jour l'interface
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling quiz update: {ex.Message}");
            }
        }

        private async Task LoadQuizData()
        {
            if (currentPlayer == null) return;

            try
            {
                // Charger l'état du quiz
                quizState = await Http.GetFromJsonAsync<QuizState>("api/quiz/state");
                
                // Si le quiz est activé et qu'il y a une question courante
                if (quizState?.IsEnabled == true && quizState.CurrentQuestionNumber > 0)
                {
                    // Charger la question courante
                    currentQuestion = await Http.GetFromJsonAsync<Quiz>($"api/quiz/questions/{quizState.CurrentQuestionNumber}");
                    
                    // Vérifier si le joueur a déjà répondu à cette question
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

        private async Task SubmitQuizAnswer()
        {
            if (string.IsNullOrEmpty(selectedQuizAnswer) || currentPlayer == null || currentQuestion == null) return;

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
                    
                    // Recharger les données du quiz
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

        private async Task ClearMessagesAfterDelay()
        {
            await Task.Delay(5000);
            errorMessage = "";
            successMessage = "";
            StateHasChanged();
        }

        private void GoToMonEspace()
        {
            Navigation.NavigateTo("/mon-espace");
        }

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }
    }
}
