namespace ChildrenMoviesApi.Application.Interfaces;

public interface IAuthService
{
    Task<(string AccessToken, string RefreshToken)> GoogleLoginAsync(string idToken);
    Task<(string AccessToken, string RefreshToken)?> RefreshTokenAsync(string refreshToken);
    Task RevokeRefreshTokenAsync(string refreshToken);
}
