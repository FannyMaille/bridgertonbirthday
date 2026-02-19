using Microsoft.AspNetCore.Mvc;
using BridgertonGame.Shared.Models;
using BridgertonGame.Server.Services;

namespace BridgertonGame.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly GameDataService _gameData;

    public PlayersController(GameDataService gameData)
    {
        _gameData = gameData;
    }

    [HttpGet]
    public ActionResult<List<Player>> GetAll()
    {
        return Ok(_gameData.GetAllPlayers());
    }

    [HttpGet("{id}")]
    public ActionResult<Player> GetById(string id)
    {
        var player = _gameData.GetPlayerById(id);
        if (player == null)
            return NotFound();

        return Ok(player);
    }

    [HttpGet("family/{familyId}")]
    public ActionResult<List<Player>> GetByFamily(string familyId)
    {
        return Ok(_gameData.GetPlayersByFamily(familyId));
    }

    [HttpGet("by-code/{code}")]
    public ActionResult<Player> GetByCode(string code)
    {
        var player = _gameData.GetPlayerByCode(code);
        if (player == null)
            return NotFound(new { message = "Code invalide" });

        return Ok(player);
    }
}
