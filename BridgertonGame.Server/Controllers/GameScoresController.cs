using Microsoft.AspNetCore.Mvc;
using BridgertonGame.Shared.Models;
using BridgertonGame.Server.Services;

namespace BridgertonGame.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameScoresController : ControllerBase
{
    private readonly DatabaseGameDataService _gameData;

    public GameScoresController(DatabaseGameDataService gameData)
    {
        _gameData = gameData;
    }

    [HttpGet]
    public async Task<ActionResult<List<GameScore>>> GetAll()
    {
        var scores = await _gameData.GetAllGameScoresAsync();
        return Ok(scores);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] GameScore gameScore)
    {
        await _gameData.CreateGameScoreAsync(gameScore);
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] GameScore gameScore)
    {
        await _gameData.UpdateGameScoreAsync(gameScore);
        return Ok();
    }

    [HttpDelete("{gameName}")]
    public async Task<ActionResult> Delete(string gameName)
    {
        await _gameData.DeleteGameScoreAsync(gameName);
        return Ok();
    }

    [HttpGet("penalties")]
    public async Task<ActionResult<Dictionary<string, int>>> GetPenalties()
    {
        var penalties = await _gameData.GetPenaltiesAsync();
        return Ok(penalties);
    }

    [HttpGet("lady-whistledown-team-points")]
    public async Task<ActionResult<int>> GetLadyWhistledownTeamPoints()
    {
        var totalPoints = await _gameData.GetLadyWhistledownTeamPointsAsync();
        return Ok(totalPoints);
    }

    [HttpGet("lady-whistledown-individual-points")]
    public async Task<ActionResult<Dictionary<string, int>>> GetLadyWhistledownIndividualPoints()
    {
        var individualPoints = await _gameData.GetLadyWhistledownIndividualPointsAsync();
        return Ok(individualPoints);
    }

    [HttpPut("penalties/{familyId}")]
    public async Task<ActionResult> UpdatePenalty(string familyId, [FromBody] int penalty)
    {
        await _gameData.UpdateWhistledownPenaltyAsync(familyId, penalty);
        return Ok();
    }
}
