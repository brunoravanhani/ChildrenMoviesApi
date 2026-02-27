using ChildrenMoviesApi.Application.Intefaces;
using ChildrenMoviesApi.Core.Logging;
using ChildrenMoviesApi.Domain.Dtos;
using ChildrenMoviesApi.Domain.Entity;
using ChildrenMoviesApi.Domain.Interfaces.Services;

namespace ChildrenMoviesApi.Application.Services;

internal class MoviesService : IMoviesService
{
    private readonly string className = nameof(MoviesService);
    private readonly ILogger _logger;
    private readonly ISearchMovieGateway _searchMovieGateway;

    public MoviesService(ILogger logger, ISearchMovieGateway searchMovieGateway)
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