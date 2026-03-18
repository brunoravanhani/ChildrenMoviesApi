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

    public async Task<GoogleUserInfo> GetUserInfoAsync(string userId)
    {
        // This is a placeholder implementation. In production, you would need to:
        // 1. Store user info in your database when they first login
        // 2. Retrieve it here, or
        // 3. Use Google's People API to fetch the user info
        // For now, we'll return a basic user with the provided ID
        return await Task.FromResult(new GoogleUserInfo
        {
            Id = userId,
            Name = string.Empty,
            Email = string.Empty
        });
    }
}