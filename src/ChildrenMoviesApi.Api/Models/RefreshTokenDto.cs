namespace ChildrenMoviesApi.Api.Models;

public class RefreshTokenDto
{
    public string RefreshToken { get; set; } = string.Empty;
}

public class AuthTokenResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
