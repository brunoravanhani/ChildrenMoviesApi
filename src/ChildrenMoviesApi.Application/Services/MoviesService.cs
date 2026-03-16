using ChildrenMoviesApi.Application.Intefaces;
using ChildrenMoviesApi.Core.Logging;
using ChildrenMoviesApi.Domain.Dtos;
using ChildrenMoviesApi.Domain.Entity;
using ChildrenMoviesApi.Domain.Interfaces.Repositories;
using ChildrenMoviesApi.Domain.Interfaces.Services;

namespace ChildrenMoviesApi.Application.Services;

internal class MoviesService : IMoviesService
{
    private readonly string className = nameof(MoviesService);
    private readonly ILogger _logger;
    private readonly ISearchMovieGateway _searchMovieGateway;
    private readonly IUserMovieRepository _userMovieRepository;

    public MoviesService(ILogger logger, ISearchMovieGateway searchMovieGateway, IUserMovieRepository userMovieRepository)
    {
        _logger = logger;
        _searchMovieGateway = searchMovieGateway;
        _userMovieRepository = userMovieRepository;
    }

    public async Task<IEnumerable<Movie>> Search(SearchParamsDto searchParams)
    {
        string methodName = nameof(Search);

        _logger.LogInformation($"Class: {className} | Method: {methodName} | Initializing");

        var results = await _searchMovieGateway.Search(searchParams);

        return results;
    }

    public async Task<IEnumerable<GalleryMovieDto>> GetAllByUser(string userId)
    {
        string methodName = nameof(GetAllByUser);

        _logger.LogInformation($"Class: {className} | Method: {methodName} | Initializing");

        var userMovies = await _userMovieRepository.GetAllByUserAsync(userId);

        return userMovies.Select(x => Map(x)).ToList();
    }

    public async Task AddMovieAsync(AddMovieDto addMovieDto)
    {
        string methodName = nameof(AddMovieAsync);

        _logger.LogInformation($"Class: {className} | Method: {methodName} | Initializing with UserId: {addMovieDto.UserId}, MovieId: {addMovieDto.Id}");

        var movie = new Movie(
            addMovieDto.Id,
            addMovieDto.BackdropPath,
            addMovieDto.PosterPath,
            addMovieDto.OriginalLanguage,
            addMovieDto.OriginalTitle,
            addMovieDto.Overview,
            addMovieDto.ReleaseDate?.ToString("yyyy-MM-dd"),
            addMovieDto.Title);

        var userMovie = UserMovie.Create(addMovieDto.UserId, movie);

        await _userMovieRepository.AddAsync(userMovie);
        await _userMovieRepository.SaveChangesAsync();

        _logger.LogInformation($"Class: {className} | Method: {methodName} | UserMovie added successfully");
    }

    public async Task DeleteMovieAsync(int movieId, string userId)
    {
        string methodName = nameof(DeleteMovieAsync);
        _logger.LogInformation($"Class: {className} | Method: {methodName} | Initializing with MovieId: {movieId}, UserId: {userId}");

        var userMovie = await _userMovieRepository.GetByMovieAndUserAsync(movieId, userId);

        if (userMovie == null)
        {
            throw new ArgumentNullException(nameof(userMovie));
        }

        await _userMovieRepository.DeleteAsync(userMovie);
        await _userMovieRepository.SaveChangesAsync();

        _logger.LogInformation($"Class: {className} | Method: {methodName} | Movie deleted successfully");
    }

    public async Task UpdatePointsAsync(int movieId, int points, string userId)
    {
        string methodName = nameof(UpdatePointsAsync);
        _logger.LogInformation($"Class: {className} | Method: {methodName} | Initializing with MovieId: {movieId}, Points: {points}, UserId: {userId}");

        var userMovie = await _userMovieRepository.GetByMovieAndUserAsync(movieId, userId);

        if (userMovie == null)
        {
            throw new ArgumentNullException(nameof(userMovie));
        }

        userMovie.UpdatePoints(points);
        await _userMovieRepository.SaveChangesAsync();

        _logger.LogInformation($"Class: {className} | Method: {methodName} | Points updated successfully");
    }


    private static GalleryMovieDto Map(UserMovie userMovie)
    {
        return new GalleryMovieDto
        {
            Id = userMovie.Movie.Id,
            Title = userMovie.Movie.Title,
            OriginalTitle = userMovie.Movie.OriginalTitle,
            Overview = userMovie.Movie.Overview,
            PosterPath = userMovie.Movie.PosterPath,
            BackdropPath = userMovie.Movie.BackdropPath,
            OriginalLanguage = userMovie.Movie.OriginalLanguage,
            ReleaseDate = userMovie.Movie.ReleaseDate,
            Points = userMovie.Points
        };
    }
}