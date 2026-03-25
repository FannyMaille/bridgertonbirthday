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

    public async Task<int> GetLadyWhistledownTeamPointsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<int>("api/gamescores/lady-whistledown-team-points");
        }
        catch
        {
            return 0;
        }
    }

    public async Task<Dictionary<string, int>> GetLadyWhistledownIndividualPointsAsync()
    {
        return await _httpClient.GetFromJsonAsync<Dictionary<string, int>>("api/gamescores/lady-whistledown-individual-points") ?? new();
    }

    public async Task<FamilyVoteResult?> GetVoteResultsAsync(string familyId)
    {
        return await _httpClient.GetFromJsonAsync<FamilyVoteResult>($"api/families/{familyId}/vote-results");
    }

    public async Task<List<FamilyVoteResult>> GetAllVoteResultsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<FamilyVoteResult>>("api/families/vote-results") ?? new();
    }

    public async Task<bool> DeleteVoteAsync(string familyId, string voterId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/families/{familyId}/vote/{voterId}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
