using ChildrenMoviesApi.Core.Configuration;
using ChildrenMoviesApi.Domain.Interfaces.Services;
using ChildrenMoviesApi.Infra.Google.Models;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace ChildrenMoviesApi.Infra.Google;

internal class GoogleAuthService : IGoogleAuthService
{
    private readonly GoogleCredentials _googleCredentials;

    public GoogleAuthService(GoogleCredentials googleCredentials)
    {
        _googleCredentials = googleCredentials;
    }

    public async Task<GoogleUserInfo> ValidateTokenAsync(string idToken)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(
            idToken,
            new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _googleCredentials.ClientId }
            });

        return new GoogleUserInfo
        {
            Id = payload.Subject,
            Name = payload.Name,
            Email = payload.Email
        };
    }
}