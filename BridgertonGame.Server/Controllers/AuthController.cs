using Microsoft.AspNetCore.Mvc;
using BridgertonGame.Shared.DTOs;
using BridgertonGame.Server.Services;

namespace BridgertonGame.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly DatabaseGameDataService _gameData;

    public AuthController(DatabaseGameDataService gameData)
    {
        _gameData = gameData;
    }

    [HttpPost("admin")]
    public async Task<ActionResult<AdminLoginResponse>> AdminLogin([FromBody] AdminLoginRequest request)
    {
        var isValid = await _gameData.ValidateAdminAsync(request.Username, request.Password);
        
        if (isValid)
        {
            return Ok(new AdminLoginResponse
            {
                Success = true,
                Token = "admin-token"
            });
        }

        return Unauthorized(new AdminLoginResponse
        {
            Success = false,
            ErrorMessage = "Login ou mot de passe incorrect"
        });
    }
}
