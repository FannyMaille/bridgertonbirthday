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

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, [FromBody] Player player)
    {
        if (id != player.Id)
            return BadRequest(new { message = "L'ID ne correspond pas" });

        var updated = await _gameData.UpdatePlayerAsync(player);
        if (!updated)
            return NotFound(new { message = "Joueur non trouvé" });

        return Ok(new { message = "Joueur mis à jour avec succès" });
    }

    [HttpPost]
    public async Task<ActionResult<Player>> Create([FromBody] Player player)
    {
        // Générer un nouvel ID si vide
        if (string.IsNullOrEmpty(player.Id))
        {
            player.Id = Guid.NewGuid().ToString();
        }

        var created = await _gameData.AddPlayerAsync(player);
        if (!created)
            return BadRequest(new { message = "Erreur lors de la création du joueur" });

        return CreatedAtAction(nameof(GetByCode), new { code = player.Code }, player);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var deleted = await _gameData.DeletePlayerAsync(id);
        if (!deleted)
            return NotFound(new { message = "Joueur non trouvé" });

        return Ok(new { message = "Joueur supprimé avec succès" });
    }
}
