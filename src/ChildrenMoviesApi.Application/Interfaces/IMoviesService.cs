using ChildrenMoviesApi.Domain.Dtos;
using ChildrenMoviesApi.Domain.Entity;

namespace ChildrenMoviesApi.Application.Intefaces;

public interface IMoviesService
{
    Task<IEnumerable<Movie>> Search(SearchParamsDto searchParams);
    Task<IEnumerable<GalleryMovieDto>> GetAllByUser(string userId);
    Task AddMovieAsync(AddMovieDto addMovieDto);
    Task DeleteMovieAsync(int movieId, string userId);
    Task UpdatePointsAsync(int movieId, int points, string userId);
}

