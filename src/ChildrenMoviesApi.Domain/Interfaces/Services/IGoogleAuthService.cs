using ChildrenMoviesApi.Infra.Google.Models;

namespace ChildrenMoviesApi.Domain.Interfaces.Services;

public interface IGoogleAuthService
{
    Task<GoogleUserInfo> ValidateTokenAsync(string idToken);
}
