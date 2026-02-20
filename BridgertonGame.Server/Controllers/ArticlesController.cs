using Microsoft.AspNetCore.Mvc;
using BridgertonGame.Shared.Models;
using BridgertonGame.Shared.DTOs;
using BridgertonGame.Server.Services;

namespace BridgertonGame.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly DatabaseGameDataService _gameData;

    public ArticlesController(DatabaseGameDataService gameData)
    {
        _gameData = gameData;
    }

    [HttpGet]
    public async Task<ActionResult<List<Article>>> GetAll()
    {
        var articles = await _gameData.GetAllArticlesAsync();
        return Ok(articles);
    }

    [HttpGet("family/{familyId}")]
    public async Task<ActionResult<List<Article>>> GetByFamily(string familyId)
    {
        var articles = await _gameData.GetArticlesByFamilyAsync(familyId);
        return Ok(articles);
    }

    [HttpPost]
    public async Task<ActionResult<PublishArticleResponse>> Publish([FromBody] PublishArticleRequest request)
    {
        var family = await _gameData.GetFamilyByIdAsync(request.FamilyId);
        if (family == null)
            return NotFound(new PublishArticleResponse 
            { 
                Success = false, 
                ErrorMessage = "Famille non trouvée" 
            });

        if (!await _gameData.CanPublishAsync(request.FamilyId))
        {
            var timeRemaining = await _gameData.GetTimeUntilNextPublicationAsync(request.FamilyId);
            return BadRequest(new PublishArticleResponse
            {
                Success = false,
                ErrorMessage = "Veuillez attendre avant de publier à nouveau",
                TimeUntilNext = timeRemaining
            });
        }

        var article = await _gameData.PublishArticleAsync(
            request.Title,
            request.Content,
            request.FamilyId,
            family.Name
        );

        return Ok(new PublishArticleResponse
        {
            Success = true,
            Article = article
        });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        await _gameData.DeleteArticleAsync(id);
        return Ok();
    }

    [HttpGet("can-publish/{familyId}")]
    public async Task<ActionResult<object>> CanPublish(string familyId)
    {
        var canPublish = await _gameData.CanPublishAsync(familyId);
        var timeRemaining = await _gameData.GetTimeUntilNextPublicationAsync(familyId);

        return Ok(new
        {
            canPublish,
            timeRemaining
        });
    }
}
