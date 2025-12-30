using ChildrenMoviesApi.Domain.Entity;

namespace ChildrenMoviesApi.Domain.Interfaces;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetAll();
}