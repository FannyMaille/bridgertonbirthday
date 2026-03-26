using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components;
using BridgertonGame.Shared.Models;

namespace BridgertonGame.Client.Services;

public class NotificationService : IAsyncDisposable
{
    private readonly HubConnection _hubConnection;
    private readonly List<Notification> _notifications = new();

    public event Func<Notification, Task>? OnNotificationReceived;
    public event Action? OnNotificationsChanged;
    public event Func<string, Task>? OnArticleDeleted;

    public IReadOnlyList<Notification> Notifications => _notifications.AsReadOnly();

    public NotificationService(NavigationManager navigationManager)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(navigationManager.ToAbsoluteUri("/notificationHub"), options =>
            {
                options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets | 
                                   Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling;
            })
            .WithAutomaticReconnect(new[] { 
                TimeSpan.Zero, 
                TimeSpan.FromSeconds(2), 
                TimeSpan.FromSeconds(5), 
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(30)
            })
            .Build();

        _hubConnection.On<string, string, string, string?, string?>("ReceiveNotification", 
            async (title, message, type, articleId, familyName) =>
            {
                var notification = new Notification
                {
                    Title = title,
                    Message = message,
                    Type = type,
                    ArticleId = articleId,
                    FamilyName = familyName,
                    CreatedAt = DateTime.UtcNow
                };

                _notifications.Insert(0, notification);
                
                // Garder seulement les 20 dernières notifications
                if (_notifications.Count > 20)
                {
                    _notifications.RemoveAt(_notifications.Count - 1);
                }

                if (OnNotificationReceived != null)
                {
                    await OnNotificationReceived.Invoke(notification);
                }
                
                OnNotificationsChanged?.Invoke();
            });

        // Écouter les suppressions d'articles
        _hubConnection.On<string>("ArticleDeleted", async (articleId) =>
        {
            if (OnArticleDeleted != null)
            {
                await OnArticleDeleted.Invoke(articleId);
            }
        });

        // Log connection state changes
        _hubConnection.Reconnecting += error =>
        {
            Console.WriteLine($"NotificationHub reconnecting: {error?.Message}");
            return Task.CompletedTask;
        };

        _hubConnection.Reconnected += connectionId =>
        {
            Console.WriteLine($"NotificationHub reconnected: {connectionId}");
            return Task.CompletedTask;
        };

        _hubConnection.Closed += error =>
        {
            Console.WriteLine($"NotificationHub closed: {error?.Message}");
            return Task.CompletedTask;
        };
    }

    public async Task StartAsync()
    {
        if (_hubConnection.State == HubConnectionState.Disconnected)
        {
            try
            {
                await _hubConnection.StartAsync();
                Console.WriteLine("NotificationHub connected successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"NotificationHub connection error: {ex.Message}");
            }
        }
    }

    public async Task StopAsync()
    {
        if (_hubConnection.State != HubConnectionState.Disconnected)
        {
            await _hubConnection.StopAsync();
        }
    }

    public void MarkAsRead(string notificationId)
    {
        var notification = _notifications.FirstOrDefault(n => n.Id == notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            OnNotificationsChanged?.Invoke();
        }
    }

    public void MarkAllAsRead()
    {
        foreach (var notification in _notifications)
        {
            notification.IsRead = true;
        }
        OnNotificationsChanged?.Invoke();
    }

    public void ClearNotification(string notificationId)
    {
        var notification = _notifications.FirstOrDefault(n => n.Id == notificationId);
        if (notification != null)
        {
            _notifications.Remove(notification);
            OnNotificationsChanged?.Invoke();
        }
    }

    public void ClearAll()
    {
        _notifications.Clear();
        OnNotificationsChanged?.Invoke();
    }

    public int GetUnreadCount() => _notifications.Count(n => !n.IsRead);

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.DisposeAsync();
    }
}
