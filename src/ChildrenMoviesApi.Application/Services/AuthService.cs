using ChildrenMoviesApi.Application.Interfaces;
using ChildrenMoviesApi.Application.Interfaces.Helper;
using ChildrenMoviesApi.Domain.Entity;
using ChildrenMoviesApi.Domain.Interfaces.Repositories;
using ChildrenMoviesApi.Domain.Interfaces.Services;

namespace ChildrenMoviesApi.Application.Services;

internal class AuthService : IAuthService
{
    private readonly IGoogleAuthService _googleAuthService;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthService(
        IGoogleAuthService googleAuthService,
        ITokenService tokenService,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _googleAuthService = googleAuthService;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<(string AccessToken, string RefreshToken)> GoogleLoginAsync(string idToken)
    {
        var user = await _googleAuthService.ValidateTokenAsync(idToken);

        var accessToken = _tokenService.GenerateToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshToken,
            ExpiryDate = DateTime.UtcNow.AddDays(7),
            CreatedDate = DateTime.UtcNow
        };

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);
        await _refreshTokenRepository.SaveChangesAsync();

        return (accessToken, refreshToken);
    }

    public async Task<(string AccessToken, string RefreshToken)?> RefreshTokenAsync(string refreshToken)
    {
        var storedRefreshToken = await _refreshTokenRepository.GetValidTokenAsync(refreshToken);

        if (storedRefreshToken == null)
            return null;

        var user = await _googleAuthService.GetUserInfoAsync(storedRefreshToken.UserId);

        var newAccessToken = _tokenService.GenerateToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        var newRefreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = storedRefreshToken.UserId,
            Token = newRefreshToken,
            ExpiryDate = DateTime.UtcNow.AddDays(7),
            CreatedDate = DateTime.UtcNow
        };

        await _refreshTokenRepository.AddAsync(newRefreshTokenEntity);
        await _refreshTokenRepository.RevokeAsync(refreshToken);
        await _refreshTokenRepository.SaveChangesAsync();

        return (newAccessToken, newRefreshToken);
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken)
    {
        await _refreshTokenRepository.RevokeAsync(refreshToken);
        await _refreshTokenRepository.SaveChangesAsync();
    }
}
