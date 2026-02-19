using Blazored.LocalStorage;
using BridgertonGame.Shared.Models;

namespace BridgertonGame.Client.Services;

public class AuthService
{
    private readonly ILocalStorageService _localStorage;
    private const string CurrentPlayerKey = "bridgerton_currentPlayer";
    private const string IsAdminKey = "bridgerton_isAdmin";

    public AuthService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<Player?> GetCurrentPlayerAsync()
    {
        return await _localStorage.GetItemAsync<Player>(CurrentPlayerKey);
    }

    public async Task SetCurrentPlayerAsync(Player? player)
    {
        if (player != null)
            await _localStorage.SetItemAsync(CurrentPlayerKey, player);
        else
            await _localStorage.RemoveItemAsync(CurrentPlayerKey);
    }

    public async Task<bool> IsAdminAuthenticatedAsync()
    {
        return await _localStorage.GetItemAsync<bool>(IsAdminKey);
    }

    public async Task SetAdminAuthenticatedAsync(bool value)
    {
        if (value)
            await _localStorage.SetItemAsync(IsAdminKey, true);
        else
            await _localStorage.RemoveItemAsync(IsAdminKey);
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(CurrentPlayerKey);
        await _localStorage.RemoveItemAsync(IsAdminKey);
    }
}
