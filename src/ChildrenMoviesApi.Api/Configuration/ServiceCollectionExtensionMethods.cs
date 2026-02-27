using ChildrenMoviesApi.Core.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChildrenMoviesApi.Api.Configuration;

public static class ServiceCollectionExtensionMethods
{
    public static void AddBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services
        .AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
            };
        });
    }

    public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var tmdbCredentials = new TmdbCredentials();
        configuration.GetSection("Tmdb").Bind(tmdbCredentials);
        services.AddSingleton(tmdbCredentials);

        var googleCredentials = new GoogleCredentials();
        configuration.GetSection("Google").Bind(googleCredentials);
        services.AddSingleton(googleCredentials);

        var jwtCredentials = new JwtCredentials();
        configuration.GetSection("Jwt").Bind(jwtCredentials);
        services.AddSingleton(jwtCredentials);

        return services;
    }
}
