using ChildrenMoviesApi.Domain.Entity;

namespace ChildrenMoviesApi.Application.Intefaces;

public interface IMoviesApplication
{
    Task<IEnumerable<Movie>> QueryMovies();
    Task<Movie> GetMovie(Guid id);
    Task SaveMovie(Movie movie);
    Task UpdateMovie(Guid id, Movie movie);
}