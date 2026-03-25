using Microsoft.AspNetCore.SignalR.Client;
using BridgertonGame.Shared.Models;
using System.Net.Http.Json;

namespace BridgertonGame.Client.Services;

public class ChatService : IAsyncDisposable
{
    private readonly HttpClient _httpClient;
    private HubConnection? _hubConnection;

    public event Action<ChatMessage>? OnMessageReceived;
    public event Action? OnMessagesCleared;

    public ChatService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task InitializeAsync()
    {
        if (_hubConnection != null)
            return;

        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{_httpClient.BaseAddress}chatHub")
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<ChatMessage>("ReceiveMessage", (message) =>
        {
            OnMessageReceived?.Invoke(message);
        });

        _hubConnection.On("MessagesCleared", () =>
        {
            OnMessagesCleared?.Invoke();
        });

        await _hubConnection.StartAsync();
    }

    public async Task<List<ChatMessage>> GetMessagesAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<ChatMessage>>("api/chat");
            return response ?? new List<ChatMessage>();
        }
        catch
        {
            return new List<ChatMessage>();
        }
    }

    public async Task<bool> SendMessageAsync(string senderId, string content)
    {
        try
        {
            var request = new SendChatMessageRequest
            {
                SenderId = senderId,
                Content = content
            };

            var response = await _httpClient.PostAsJsonAsync("api/chat", request);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<int> GetMessageCountAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<int>("api/chat/count");
            return response;
        }
        catch
        {
            return 0;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.DisposeAsync();
        }
    }
}
