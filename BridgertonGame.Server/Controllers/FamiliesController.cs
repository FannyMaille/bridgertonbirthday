using Microsoft.AspNetCore.Mvc;
using BridgertonGame.Shared.Models;
using BridgertonGame.Shared.DTOs;
using BridgertonGame.Server.Services;

namespace BridgertonGame.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FamiliesController : ControllerBase
{
    private readonly GameDataService _gameData;

    public FamiliesController(GameDataService gameData)
    {
        _gameData = gameData;
    }

    [HttpGet]
    public ActionResult<List<Family>> GetAll()
    {
        return Ok(_gameData.GetAllFamilies());
    }

    [HttpGet("{id}")]
    public ActionResult<Family> GetById(string id)
    {
        var family = _gameData.GetFamilyById(id);
        if (family == null)
            return NotFound();

        return Ok(family);
    }

    [HttpPost("{id}/vote")]
    public ActionResult Vote(string id, [FromBody] VoteRequest request)
    {
        var family = _gameData.GetFamilyById(id);
        if (family == null)
            return NotFound();

        _gameData.SetLadyWhistledown(id, request.PlayerId);
        return Ok(new { message = "Vote enregistré" });
    }

    [HttpPost("{id}/toggle-voting")]
    public ActionResult ToggleVoting(string id, [FromBody] bool enabled)
    {
        var family = _gameData.GetFamilyById(id);
        if (family == null)
            return NotFound();

        _gameData.ToggleVoting(id, enabled);
        return Ok();
    }

    [HttpPost("{id}/reveal")]
    public ActionResult Reveal(string id)
    {
        var family = _gameData.GetFamilyById(id);
        if (family == null)
            return NotFound();

        _gameData.RevealLadyWhistledown(id);
        return Ok();
    }

    [HttpPost("reveal-all")]
    public ActionResult RevealAll()
    {
        var families = _gameData.GetAllFamilies();
        foreach (var family in families)
        {
            _gameData.RevealLadyWhistledown(family.Id);
        }
        return Ok();
    }
}
