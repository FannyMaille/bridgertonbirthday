using Microsoft.AspNetCore.Mvc;
using BridgertonGame.Shared.DTOs;

namespace BridgertonGame.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private const string AdminUsername = "admin";
    private const string AdminPassword = "bridgerton2024";

    [HttpPost("admin")]
    public ActionResult<AdminLoginResponse> AdminLogin([FromBody] AdminLoginRequest request)
    {
        if (request.Username == AdminUsername && request.Password == AdminPassword)
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
