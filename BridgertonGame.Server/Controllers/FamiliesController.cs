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

        // Check if voting is enabled for this family
        if (!family.VotingEnabled)
            return BadRequest(new { message = "Le vote n'est pas activé pour cette famille" });

        // Save the vote
        await _gameData.SaveVoteAsync(id, request.VoterId, request.PlayerId);
        
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

    [HttpGet("{id}/vote-results")]
    public async Task<ActionResult<FamilyVoteResult>> GetVoteResults(string id)
    {
        var family = await _gameData.GetFamilyByIdAsync(id);
        if (family == null)
            return NotFound();

        var results = await _gameData.GetVoteResultsAsync(id);
        return Ok(results);
    }

    [HttpGet("vote-results")]
    public async Task<ActionResult<List<FamilyVoteResult>>> GetAllVoteResults()
    {
        var results = await _gameData.GetAllVoteResultsAsync();
        return Ok(results);
    }

    [HttpDelete("{familyId}/vote/{voterId}")]
    public async Task<ActionResult> DeleteVote(string familyId, string voterId)
    {
        var family = await _gameData.GetFamilyByIdAsync(familyId);
        if (family == null)
            return NotFound();

        var success = await _gameData.DeleteVoteAsync(familyId, voterId);
        if (!success)
            return NotFound(new { message = "Vote non trouvé" });

        return Ok(new { message = "Vote supprimé avec succès" });
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

    [HttpDelete("{id}/timer")]
    public async Task<ActionResult> ResetTimer(string id)
    {
        var family = await _gameData.GetFamilyByIdAsync(id);
        if (family == null)
            return NotFound();

        await _gameData.ResetPublicationTimerAsync(id);
        return Ok(new { message = "Timer réinitialisé" });
    }

    [HttpPost("{id}/timer/set")]
    public async Task<ActionResult> SetTimer(string id, [FromBody] SetTimerRequest request)
    {
        var family = await _gameData.GetFamilyByIdAsync(id);
        if (family == null)
            return NotFound();

        await _gameData.SetPublicationTimerAsync(id, request.Minutes);
        return Ok(new { message = $"Timer défini à {request.Minutes} minutes" });
    }

    [HttpGet("{id}/timer/status")]
    public async Task<ActionResult<TimerStatusDto>> GetTimerStatus(string id)
    {
        var family = await _gameData.GetFamilyByIdAsync(id);
        if (family == null)
            return NotFound();

        var canPublish = await _gameData.CanPublishAsync(id);
        var timeUntilNext = await _gameData.GetTimeUntilNextPublicationAsync(id);
        var lastPublicationTime = await _gameData.GetLastPublicationTimeAsync(id);

        return Ok(new TimerStatusDto
        {
            CanPublish = canPublish,
            TimeUntilNext = timeUntilNext,
            LastPublicationTime = lastPublicationTime
        });
    }
}

public class SetTimerRequest
{
    public int Minutes { get; set; }
}

public class TimerStatusDto
{
    public bool CanPublish { get; set; }
    public TimeSpan? TimeUntilNext { get; set; }
    public DateTime? LastPublicationTime { get; set; }
}
