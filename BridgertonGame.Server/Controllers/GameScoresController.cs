using Microsoft.AspNetCore.Mvc;
using BridgertonGame.Shared.Models;
using BridgertonGame.Server.Services;

namespace BridgertonGame.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameScoresController : ControllerBase
{
    private readonly GameDataService _gameData;

    public GameScoresController(GameDataService gameData)
    {
        _gameData = gameData;
    }

    [HttpGet]
    public ActionResult<List<GameScore>> GetAll()
    {
        return Ok(_gameData.GetAllGameScores());
    }

    [HttpPut]
    public ActionResult Update([FromBody] GameScore gameScore)
    {
        foreach (var familyScore in gameScore.FamilyScores)
        {
            _gameData.UpdateGameScore(gameScore.GameName, familyScore.Key, familyScore.Value);
        }
        return Ok();
    }

    [HttpGet("penalties")]
    public ActionResult<Dictionary<string, int>> GetPenalties()
    {
        return Ok(_gameData.GetWhistledownPenalties());
    }

    [HttpPut("penalties/{familyId}")]
    public ActionResult UpdatePenalty(string familyId, [FromBody] int penalty)
    {
        _gameData.UpdateWhistledownPenalty(familyId, penalty);
        return Ok();
    }
}
