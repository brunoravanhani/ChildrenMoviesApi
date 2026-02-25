using ChildrenMoviesApi.Domain.Interfaces.Gateways;
using ChildrenMoviesApi.Infra.Tmdb.Factory;
using ChildrenMoviesApi.Infra.Tmdb.Factory.Interfaces;
using ChildrenMoviesApi.Infra.Tmdb.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ChildrenMoviesApi.Infra.Tmdb.Configuration;

public static class InitializeTmdb
{
    public static IServiceCollection TmdbDI(this IServiceCollection services)
    {
        services.AddScoped<ISearchMovieGateway, SearchMovieGateway>();
        services.AddScoped<IHttpClientFactory, HttpClientFactory>();

        return services;
    }
}
