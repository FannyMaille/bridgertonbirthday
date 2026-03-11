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

    [HttpPost("{id}/set-whistledown")]
    public async Task<ActionResult> SetLadyWhistledown(string id, [FromBody] SetWhistledownRequest request)
    {
        var family = await _gameData.GetFamilyByIdAsync(id);
        if (family == null)
            return NotFound();

        await _gameData.SetLadyWhistledownAsync(id, request.PlayerId);
        return Ok(new { message = "Lady Whistledown mise à jour" });
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

    [HttpPost("{id}/toggle-reveal")]
    public async Task<ActionResult> ToggleReveal(string id, [FromBody] bool revealed)
    {
        var family = await _gameData.GetFamilyByIdAsync(id);
        if (family == null)
            return NotFound();

        await _gameData.ToggleRevealLadyWhistledownAsync(id, revealed);
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

    [HttpPost]
    public async Task<ActionResult<Family>> Create([FromBody] Family family)
    {
        if (string.IsNullOrWhiteSpace(family.Name))
            return BadRequest("Le nom de la famille est requis");

        await _gameData.CreateFamilyAsync(family);
        return CreatedAtAction(nameof(GetById), new { id = family.Id }, family);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, [FromBody] Family family)
    {
        if (id != family.Id)
            return BadRequest("L'ID de la famille ne correspond pas");

        var existingFamily = await _gameData.GetFamilyByIdAsync(id);
        if (existingFamily == null)
            return NotFound();

        await _gameData.UpdateFamilyAsync(family);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var family = await _gameData.GetFamilyByIdAsync(id);
        if (family == null)
            return NotFound();

        var success = await _gameData.DeleteFamilyAsync(id);
        if (!success)
            return BadRequest("Impossible de supprimer la famille, elle contient encore des membres");

        return Ok();
    }
}
