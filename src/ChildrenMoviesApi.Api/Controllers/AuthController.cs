using ChildrenMoviesApi.Api.Models;
using ChildrenMoviesApi.Application.Interfaces;
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
        var jwt = await _authService.GoogleLoginAsync(request.IdToken);

        return Ok(new { Token = jwt });
    }


}
