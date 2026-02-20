using Microsoft.AspNetCore.Mvc;
using BridgertonGame.Shared.Models;
using BridgertonGame.Shared.DTOs;
using BridgertonGame.Server.Services;

namespace BridgertonGame.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FamiliesController : ControllerBase
{
    private readonly DatabaseGameDataService _gameData;

    public FamiliesController(DatabaseGameDataService gameData)
    {
        _gameData = gameData;
    }

    [HttpGet]
    public async Task<ActionResult<List<Family>>> GetAll()
    {
        var families = await _gameData.GetAllFamiliesAsync();
        return Ok(families);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Family>> GetById(string id)
    {
        var family = await _gameData.GetFamilyByIdAsync(id);
        if (family == null)
            return NotFound();

        return Ok(family);
    }

    [HttpPost("{id}/vote")]
    public async Task<ActionResult> Vote(string id, [FromBody] VoteRequest request)
    {
        var family = await _gameData.GetFamilyByIdAsync(id);
        if (family == null)
            return NotFound();

        await _gameData.SetLadyWhistledownAsync(id, request.PlayerId);
        return Ok(new { message = "Vote enregistré" });
    }

    [HttpPost("{id}/toggle-voting")]
    public async Task<ActionResult> ToggleVoting(string id, [FromBody] bool enabled)
    {
        var family = await _gameData.GetFamilyByIdAsync(id);
        if (family == null)
            return NotFound();

        await _gameData.ToggleVotingAsync(id, enabled);
        return Ok();
    }

    [HttpPost("{id}/reveal")]
    public async Task<ActionResult> Reveal(string id)
    {
        var family = await _gameData.GetFamilyByIdAsync(id);
        if (family == null)
            return NotFound();

        await _gameData.RevealLadyWhistledownAsync(id);
        return Ok();
    }

    [HttpPost("reveal-all")]
    public async Task<ActionResult> RevealAll()
    {
        var families = await _gameData.GetAllFamiliesAsync();
        foreach (var family in families)
        {
            await _gameData.RevealLadyWhistledownAsync(family.Id);
        }
        return Ok();
    }
}
