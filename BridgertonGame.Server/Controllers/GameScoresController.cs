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

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] GameScore gameScore)
    {
        await _gameData.UpdateGameScoreAsync(gameScore);
        return Ok();
    }

    [HttpGet("penalties")]
    public async Task<ActionResult<Dictionary<string, int>>> GetPenalties()
    {
        var penalties = await _gameData.GetPenaltiesAsync();
        return Ok(penalties);
    }

    [HttpPut("penalties/{familyId}")]
    public async Task<ActionResult> UpdatePenalty(string familyId, [FromBody] int penalty)
    {
        await _gameData.UpdateWhistledownPenaltyAsync(familyId, penalty);
        return Ok();
    }
}
