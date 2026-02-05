using CountryCity.Api.Auth;
using CountryCity.Application.Abstractions;
using CountryCity.Application.Auth;
using Microsoft.AspNetCore.Mvc;

namespace CountryCity.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly TokenService _tokens;
    private readonly IUserRepository _users;

    public AuthController(TokenService tokens, IUserRepository users)
    {
        _tokens = tokens;
        _users = users;
    }

    [HttpPost("token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Token([FromBody] TokenRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
        {
            return Unauthorized(new { message = "Invalid credentials." });
        }

        var user = await _users.GetByUsernameAsync(req.Username, ct);

        if (user is null)
        {
            return Unauthorized(new { message = "Invalid credentials." });
        }

        if (!PasswordHasher.Verify(req.Password, user.PasswordHash, user.PasswordSalt))
        {
            return Unauthorized(new { message = "Invalid credentials." });
        }

        var token = _tokens.CreateToken(user.Username);
        return Ok(new { access_token = token, token_type = "Bearer" });
    }
}

public sealed record TokenRequest(string Username, string Password);
