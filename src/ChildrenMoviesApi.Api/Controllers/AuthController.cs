using ChildrenMoviesApi.Api.Models;
using ChildrenMoviesApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenMoviesApi.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("google")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto request)
    {
        var (accessToken, refreshToken) = await _authService.GoogleLoginAsync(request.IdToken);

        var response = new AuthTokenResponseDto
        {
            Token = accessToken,
            RefreshToken = refreshToken
        };

        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto request)
    {
        var result = await _authService.RefreshTokenAsync(request.RefreshToken);

        if (result == null)
        {
            return Unauthorized(new { message = "Invalid or expired refresh token" });
        }

        var (accessToken, newRefreshToken) = result.Value;

        var response = new AuthTokenResponseDto
        {
            Token = accessToken,
            RefreshToken = newRefreshToken
        };

        return Ok(response);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenDto request)
    {
        await _authService.RevokeRefreshTokenAsync(request.RefreshToken);

        return Ok(new { message = "Logged out successfully" });
    }
}
