using ChildrenMoviesApi.Application.Helper;
using ChildrenMoviesApi.Application.Intefaces;
using ChildrenMoviesApi.Application.Interfaces;
using ChildrenMoviesApi.Application.Interfaces.Helper;
using ChildrenMoviesApi.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ChildrenMoviesApi.Application;

public static class ServiceCollectionExtensionMethods
{
    public static IServiceCollection ApplicationDI(this IServiceCollection services)
    {
        services.AddScoped<IMoviesService, MoviesService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
