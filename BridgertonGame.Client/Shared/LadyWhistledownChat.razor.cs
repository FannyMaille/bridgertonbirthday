using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BridgertonGame.Client.Services;
using BridgertonGame.Shared.Models;

namespace BridgertonGame.Client.Shared
{
    public partial class LadyWhistledownChat : IAsyncDisposable
    {
        [Parameter]
        public string CurrentPlayerId { get; set; } = string.Empty;

        [Inject] private ChatService ChatService { get; set; } = default!;

        private List<ChatMessage> messages = new();
        private string newMessage = string.Empty;
        private string currentPlayerId = string.Empty;
        private ElementReference messagesContainer;

        private bool CanSend => !string.IsNullOrWhiteSpace(newMessage) && newMessage.Length <= 300;

        protected override async Task OnInitializedAsync()
        {
            currentPlayerId = CurrentPlayerId;
            
            await ChatService.InitializeAsync();
            
            ChatService.OnMessageReceived += OnMessageReceived;
            ChatService.OnMessagesCleared += OnMessagesCleared;
            
            await LoadMessages();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                await ScrollToBottom();
            }
        }

        private async Task LoadMessages()
        {
            messages = await ChatService.GetMessagesAsync();
            StateHasChanged();
        }

        private async Task SendMessage()
        {
            if (!CanSend) return;

            var content = newMessage.Trim();
            newMessage = string.Empty;

            var success = await ChatService.SendMessageAsync(currentPlayerId, content);
            if (success)
            {
                StateHasChanged();
            }
        }

        private async Task HandleKeyPress(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs e)
        {
            if (e.Key == "Enter" && !e.ShiftKey && CanSend)
            {
                await SendMessage();
            }
        }

        private void OnMessageReceived(ChatMessage message)
        {
            messages.Add(message);
            InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }

        private void OnMessagesCleared()
        {
            messages.Clear();
            InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }

        private async Task ScrollToBottom()
        {
            // Scroll to bottom logic would go here if needed
            await Task.CompletedTask;
        }

        private string FormatTime(DateTime dateTime)
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

        public async ValueTask DisposeAsync()
        {
            ChatService.OnMessageReceived -= OnMessageReceived;
            ChatService.OnMessagesCleared -= OnMessagesCleared;
        }
    }
}
