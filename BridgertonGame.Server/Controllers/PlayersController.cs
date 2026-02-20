using Microsoft.AspNetCore.Mvc;
using BridgertonGame.Shared.Models;
using BridgertonGame.Server.Services;

namespace BridgertonGame.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly DatabaseGameDataService _gameData;

    public PlayersController(DatabaseGameDataService gameData)
    {
        _gameData = gameData;
    }

    [HttpGet]
    public async Task<ActionResult<List<Player>>> GetAll()
    {
        var players = await _gameData.GetAllPlayersAsync();
        return Ok(players);
    }

    [HttpGet("family/{familyId}")]
    public async Task<ActionResult<List<Player>>> GetByFamily(string familyId)
    {
        var players = await _gameData.GetPlayersByFamilyAsync(familyId);
        return Ok(players);
    }

    [HttpGet("by-code/{code}")]
    public async Task<ActionResult<Player>> GetByCode(string code)
    {
        var player = await _gameData.GetPlayerByCodeAsync(code);
        if (player == null)
            return NotFound(new { message = "Code invalide" });

        return Ok(player);
    }
}
