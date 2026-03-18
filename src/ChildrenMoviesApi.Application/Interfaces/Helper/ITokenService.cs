using ChildrenMoviesApi.Infra.Google.Models;

namespace ChildrenMoviesApi.Application.Interfaces.Helper;

internal interface ITokenService
{
    string GenerateToken(GoogleUserInfo user);
    string GenerateRefreshToken();
}
