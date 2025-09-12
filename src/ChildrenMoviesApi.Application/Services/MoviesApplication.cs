using ChildrenMoviesApi.Application.Infra;
using ChildrenMoviesApi.Application.Intefaces;
using ChildrenMoviesApi.Application.Logging;
using ChildrenMoviesApi.Application.Models;

namespace ChildrenMoviesApi.Application.Services;

public class MoviesApplication : IMoviesApplication
{
    private readonly ILogger _logger;

    public MoviesApplication(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<Movie>> QueryMovies()
    {
        try
        {
            _logger.LogInformation("Setting up context");

            using var dbContext = new ContextFactory();

            _logger.LogInformation("Querying data");

            var movies = await dbContext.MovieRepository.GetAll();

            _logger.LogInformation($"Found {movies.Count()} items");

            return movies;
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Error executing query");
            return [];
        }
    }
}