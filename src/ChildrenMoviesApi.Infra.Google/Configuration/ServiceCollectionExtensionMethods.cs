using ChildrenMoviesApi.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ChildrenMoviesApi.Infra.Google.Configuration;

public static class ServiceCollectionExtensionMethods
{
    public static IServiceCollection GoogleAuthDI(this IServiceCollection services)
    {
        services.AddScoped<IGoogleAuthService, GoogleAuthService>();
        return services;
    }
}
