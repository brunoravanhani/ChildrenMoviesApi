using ChildrenMoviesApi.Application.Infra;
using ChildrenMoviesApi.Application.Intefaces;
using ChildrenMoviesApi.Application.Logging;
using ChildrenMoviesApi.Domain.Entity;
using ChildrenMoviesApi.Domain.Configuration;
using Microsoft.Extensions.Options;

namespace ChildrenMoviesApi.Application.Services;

public class MoviesApplication : IMoviesApplication
{
    private readonly ILogger _logger;
    private readonly AwsCredentials _awsCredentials;

    public MoviesApplication(ILogger logger, IOptions<AwsCredentials> awsCredentials)
    {
        _logger = logger;
        _awsCredentials = awsCredentials.Value;
    }

    public async Task<IEnumerable<Movie>> QueryMovies()
    {
        try
        {
            _logger.LogInformation("Setting up context");

            using var dbContext = new ContextFactory(_awsCredentials);

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

    public async Task<Movie> GetMovie(int id)
    {
        try
        {
            _logger.LogInformation("Setting up context");

            using var dbContext = new ContextFactory(_awsCredentials);

            _logger.LogInformation("Querying data");

            var movie = await dbContext.MovieRepository.Get(id);

            _logger.LogInformation($"Found movie {movie.Name}");

            return movie;

        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Error executing query");
            throw;
        }
    }

    public async Task SaveMovie(Movie movie)
    {
        try
        {
            _logger.LogInformation("Setting up context");

            using var dbContext = new ContextFactory(_awsCredentials);

            _logger.LogInformation("Querying data");

            var movies = await dbContext.MovieRepository.GetAll();

            _logger.LogInformation($"Found {movies.Count()} items");

        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Error executing query");
            throw;
        }
    }
}