using ChildrenMoviesApi.Application.Models;

namespace ChildrenMoviesApi.Application.Intefaces;

public interface IMoviesApplication
{
    Task<IEnumerable<Movie>> QueryMovies();
}