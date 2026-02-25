using Amazon.Runtime.Internal;
using ChildrenMoviesApi.Application.Intefaces;
using ChildrenMoviesApi.Core.Configuration;
using ChildrenMoviesApi.Core.Logging;
using ChildrenMoviesApi.Domain.Dtos;
using ChildrenMoviesApi.Domain.Entity;
using ChildrenMoviesApi.Domain.Interfaces.Gateways;
using System.Text.Json;
namespace ChildrenMoviesApi.Application.Services;

public class MoviesApplication : IMoviesApplication
{
    private readonly string className = nameof(MoviesApplication);
    private readonly ILogger _logger;
    private readonly ISearchMovieGateway _searchMovieGateway;

    public MoviesApplication(ILogger logger, ISearchMovieGateway searchMovieGateway)
    {
        _logger = logger;
        _searchMovieGateway = searchMovieGateway;
    }

    public async Task<IEnumerable<Movie>> Search(SearchParamsDto searchParams)
    {
        string methodName = nameof(Search);

        _logger.LogInformation($"Class: {className} | Method: {methodName} | Initializing");

        var results = await _searchMovieGateway.Search(searchParams);

        return results;
    }

}