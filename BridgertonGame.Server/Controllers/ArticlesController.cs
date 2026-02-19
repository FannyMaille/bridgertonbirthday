using Microsoft.AspNetCore.Mvc;
using BridgertonGame.Shared.Models;
using BridgertonGame.Shared.DTOs;
using BridgertonGame.Server.Services;

namespace BridgertonGame.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly GameDataService _gameData;

    public ArticlesController(GameDataService gameData)
    {
        _gameData = gameData;
    }

    [HttpGet]
    public ActionResult<List<Article>> GetAll()
    {
        return Ok(_gameData.GetAllArticles());
    }

    [HttpGet("family/{familyId}")]
    public ActionResult<List<Article>> GetByFamily(string familyId)
    {
        return Ok(_gameData.GetArticlesByFamily(familyId));
    }

    [HttpPost]
    public ActionResult<PublishArticleResponse> Publish([FromBody] PublishArticleRequest request)
    {
        var family = _gameData.GetFamilyById(request.FamilyId);
        if (family == null)
            return NotFound(new PublishArticleResponse 
            { 
                Success = false, 
                ErrorMessage = "Famille non trouvée" 
            });

        if (!_gameData.CanPublish(request.FamilyId))
        {
            var timeRemaining = _gameData.GetTimeUntilNextPublication(request.FamilyId);
            return BadRequest(new PublishArticleResponse
            {
                Success = false,
                ErrorMessage = "Veuillez attendre avant de publier à nouveau",
                TimeUntilNext = timeRemaining
            });
        }

        var article = _gameData.PublishArticle(
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
    public ActionResult Delete(string id)
    {
        _gameData.DeleteArticle(id);
        return Ok();
    }

    [HttpGet("can-publish/{familyId}")]
    public ActionResult<object> CanPublish(string familyId)
    {
        var canPublish = _gameData.CanPublish(familyId);
        var timeRemaining = _gameData.GetTimeUntilNextPublication(familyId);

        return Ok(new
        {
            canPublish,
            timeRemaining
        });
    }
}
