using ChildrenMoviesApi.Domain.Entity;

namespace ChildrenMoviesApi.Application.Intefaces;

public interface IMoviesApplication
{
    Task<IEnumerable<Movie>> QueryMovies();
    Task<Movie> GetMovie(int id);
    Task SaveMovie(Movie movie);
}