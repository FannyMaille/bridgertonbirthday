using Microsoft.JSInterop;

namespace BridgertonGame.Client.Services;

public class PushNotificationService : IAsyncDisposable
{
    private readonly IJSRuntime _jsRuntime;
    private bool _isSupported;
    private bool _isSubscribed;
    private string? _subscription;

    public PushNotificationService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> IsSupportedAsync()
    {
        try
        {
            _isSupported = await _jsRuntime.InvokeAsync<bool>("pushNotifications.isSupported");
            return _isSupported;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string?> RequestPermissionAsync()
    {
        try
        {
            var permission = await _jsRuntime.InvokeAsync<string>("pushNotifications.requestPermission");
            return permission;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur demande permission: {ex.Message}");
            return "denied";
        }
    }

    public async Task<bool> SubscribeAsync()
    {
        try
        {
            var subscription = await _jsRuntime.InvokeAsync<string>("pushNotifications.subscribe");
            if (!string.IsNullOrEmpty(subscription))
            {
                _subscription = subscription;
                _isSubscribed = true;
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur souscription: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UnsubscribeAsync()
    {
        try
        {
            var result = await _jsRuntime.InvokeAsync<bool>("pushNotifications.unsubscribe");
            if (result)
            {
                _isSubscribed = false;
                _subscription = null;
            }
            return result;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ShowNotificationAsync(string title, string body, string? icon = null)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("pushNotifications.showNotification", title, body, icon ?? "/images/LadyWithldown.png");
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool IsSubscribed => _isSubscribed;
    public string? Subscription => _subscription;

    public async ValueTask DisposeAsync()
    {
        // Cleanup si nécessaire
        await Task.CompletedTask;
    }
}
