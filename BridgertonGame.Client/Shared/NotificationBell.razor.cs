using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BridgertonGame.Client.Services;
using BridgertonGame.Shared.Models;

namespace BridgertonGame.Client.Shared
{
    public partial class NotificationBell : IDisposable
    {
        [Inject] private NotificationService NotificationService { get; set; } = default!;
        [Inject] private PushNotificationService PushNotificationService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IJSRuntime JS { get; set; } = default!;

        private bool isPanelOpen = false;
        private List<Notification> notifications = new();
        private int unreadCount = 0;
        private bool pushEnabled = false;

        protected override async Task OnInitializedAsync()
        {
            await NotificationService.StartAsync();
            NotificationService.OnNotificationReceived += HandleNewNotification;
            NotificationService.OnNotificationsChanged += RefreshNotifications;
            RefreshNotifications();
            
            // Vérifier si les push notifications sont supportées
            pushEnabled = await PushNotificationService.IsSupportedAsync();
        }

        private async Task HandleNewNotification(Notification notification)
        {
            await InvokeAsync(async () =>
            {
                RefreshNotifications();
                StateHasChanged();
                
                // Afficher une notification push si activée
                if (pushEnabled)
                {
                    await PushNotificationService.ShowNotificationAsync(
                        notification.Title,
                        notification.Message,
                        "/images/LadyWithldown.png"
                    );
                }
            });
        }

        private async Task EnablePushNotifications()
        {
            var permission = await PushNotificationService.RequestPermissionAsync();
            
            if (permission == "granted")
            {
                var subscribed = await PushNotificationService.SubscribeAsync();
                if (subscribed)
                {
                    pushEnabled = true;
                    await JS.InvokeVoidAsync("alert", "✅ Notifications push activées ! Vous recevrez désormais des notifications sur votre téléphone.");
                }
                else
                {
                    await JS.InvokeVoidAsync("alert", "❌ Erreur lors de l'activation des notifications push.");
                }
            }
            else if (permission == "denied")
            {
                await JS.InvokeVoidAsync("alert", "❌ Permission refusée. Activez les notifications dans les paramètres de votre navigateur.");
            }
            
            StateHasChanged();
        }

        private void RefreshNotifications()
        {
            notifications = NotificationService.Notifications.ToList();
            unreadCount = NotificationService.GetUnreadCount();
            StateHasChanged();
        }

        private void TogglePanel()
        {
            isPanelOpen = !isPanelOpen;
            if (isPanelOpen)
            {
                MarkAllAsRead();
            }
        }

        private void ClosePanel()
        {
            isPanelOpen = false;
        }

        private void OnNotificationClick(Notification notification)
        {
            NotificationService.MarkAsRead(notification.Id);
            
            // Si c'est une notification d'article, naviguer vers la page d'accueil avec l'ID de l'article
            if (notification.Type == "article" && !string.IsNullOrEmpty(notification.ArticleId))
            {
                ClosePanel();
                Navigation.NavigateTo($"/#article-{notification.ArticleId}", forceLoad: false);
            }
        }

        private void MarkAllAsRead()
        {
            NotificationService.MarkAllAsRead();
        }

        private void ClearAll()
        {
            NotificationService.ClearAll();
        }

        private void ClearNotification(string notificationId)
        {
            NotificationService.ClearNotification(notificationId);
        }

        private string GetNotificationClass(Notification notification)
        {
            var classes = new List<string>();
            
            if (!notification.IsRead)
                classes.Add("unread");
                
            classes.Add($"notification-{notification.Type}");
            
            return string.Join(" ", classes);
        }

        private string GetNotificationIcon(string type) => type switch
        {
            "article" => "📰",
            "success" => "✅",
            "warning" => "⚠️",
            "info" => "ℹ️",
            _ => "🔔"
        };

        private string FormatTime(DateTime dateTime)
        {
            var timeSpan = DateTime.UtcNow - dateTime;
            
            if (timeSpan.TotalMinutes < 1)
                return "À l'instant";
            if (timeSpan.TotalMinutes < 60)
                return $"Il y a {(int)timeSpan.TotalMinutes} min";
            if (timeSpan.TotalHours < 24)
                return $"Il y a {(int)timeSpan.TotalHours} h";
            
            return dateTime.ToLocalTime().ToString("dd/MM à HH:mm");
        }

        public void Dispose()
        {
            NotificationService.OnNotificationReceived -= HandleNewNotification;
            NotificationService.OnNotificationsChanged -= RefreshNotifications;
        }
    }
}
