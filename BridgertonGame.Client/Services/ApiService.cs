using BridgertonGame.Shared.Models;
using System.Net.Http.Json;

namespace BridgertonGame.Client.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Player>> GetAllPlayersAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Player>>("api/players") ?? new();
    }

    public async Task<Player?> GetPlayerByIdAsync(string id)
    {
        return await _httpClient.GetFromJsonAsync<Player>($"api/players/{id}");
    }

    public async Task<Player?> GetPlayerByCodeAsync(string code)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Player>($"api/players/by-code/{code}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<Player>> GetPlayersByFamilyAsync(string familyId)
    {
        return await _httpClient.GetFromJsonAsync<List<Player>>($"api/players/family/{familyId}") ?? new();
    }

    public async Task<List<Family>> GetAllFamiliesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Family>>("api/families") ?? new();
    }

    public async Task<Family?> GetFamilyByIdAsync(string id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Family>($"api/families/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors du chargement de la famille {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<List<Article>> GetAllArticlesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Article>>("api/articles") ?? new();
    }

    public async Task<List<Article>> GetArticlesByFamilyAsync(string familyId)
    {
        return await _httpClient.GetFromJsonAsync<List<Article>>($"api/articles/family/{familyId}") ?? new();
    }

    public async Task<List<GameScore>> GetAllGameScoresAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<GameScore>>("api/gamescores") ?? new();
    }

    public async Task<Dictionary<string, int>> GetPenaltiesAsync()
    {
        return await _httpClient.GetFromJsonAsync<Dictionary<string, int>>("api/gamescores/penalties") ?? new();
    }
}
