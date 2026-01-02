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
    private readonly DatabaseTables _databaseTables;

    public MoviesApplication(ILogger logger, IOptions<AwsCredentials> awsCredentials,
        IOptions<DatabaseTables> databaseTables )
    {
        _logger = logger;
        _awsCredentials = awsCredentials.Value;
        _databaseTables = databaseTables.Value;
    }

    public async Task<IEnumerable<Movie>> QueryMovies()
    {
        try
        {
            _logger.LogInformation("Setting up context");

            using var dbContext = new ContextFactory(_awsCredentials, _databaseTables);

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

    public async Task<Movie> GetMovie(Guid id)
    {
        try
        {
            _logger.LogInformation("Setting up context");

            using var dbContext = new ContextFactory(_awsCredentials, _databaseTables);

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

            using var dbContext = new ContextFactory(_awsCredentials, _databaseTables);

            _logger.LogInformation("Saving data");

            movie.Id = Guid.NewGuid();

            var result = await dbContext.MovieRepository.Save(movie);

            if (result)
            {
                _logger.LogInformation($"Saved Movie {movie.Name}");
                return;
            }

            throw new Exception("Error when saving Movie");

        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Error executing Save");
            throw;
        }
    }

    public async Task UpdateMovie(Guid id, Movie movie)
    {
        try
        {
            _logger.LogInformation("Setting up context");

            using var dbContext = new ContextFactory(_awsCredentials, _databaseTables);

            _logger.LogInformation("Saving data");

            movie.Id = id;

            var result = await dbContext.MovieRepository.Save(movie);

            if (result)
            {
                _logger.LogInformation($"Saved Movie {movie.Name}");
                return;
            }

            throw new Exception("Error when saving Movie");

        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Error executing Save");
            throw;
        }
    }
}