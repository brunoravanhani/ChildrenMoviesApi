using ChildrenMoviesApi.Application.Interfaces;
using ChildrenMoviesApi.Application.Interfaces.Helper;
using ChildrenMoviesApi.Domain.Interfaces.Services;

namespace ChildrenMoviesApi.Application.Services;

internal class AuthService : IAuthService
{
    private readonly IGoogleAuthService _googleAuthService;
    private readonly ITokenService _tokenService;

    public AuthService(
        IGoogleAuthService googleAuthService,
        ITokenService tokenService)
    {
        _googleAuthService = googleAuthService;
        _tokenService = tokenService;
    }

    public async Task<string> GoogleLoginAsync(string idToken)
    {
        var user = await _googleAuthService.ValidateTokenAsync(idToken);

        var jwt = _tokenService.GenerateToken(user);

        return jwt;
    }
}
