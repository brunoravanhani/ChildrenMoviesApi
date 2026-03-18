using ChildrenMoviesApi.Application.Interfaces.Helper;
using ChildrenMoviesApi.Core.Configuration;
using ChildrenMoviesApi.Infra.Google.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ChildrenMoviesApi.Application.Helper;

internal class TokenService : ITokenService
{
    private readonly JwtCredentials _jwtCredentials;

    public TokenService(JwtCredentials jwtCredentials)
    {
        _jwtCredentials = jwtCredentials;
    }

    public string GenerateToken(GoogleUserInfo user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtCredentials.Key));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtCredentials.Issuer,
            audience: _jwtCredentials.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_jwtCredentials.ExpirationInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        return Convert.ToBase64String(randomNumber);
    }
}
