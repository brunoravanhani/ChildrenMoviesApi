using ChildrenMoviesApi.Domain.Entity;

namespace ChildrenMoviesApi.Application.Intefaces;

public interface IMoviesApplication
{
    Task<IEnumerable<Movie>> QueryMovies();
}