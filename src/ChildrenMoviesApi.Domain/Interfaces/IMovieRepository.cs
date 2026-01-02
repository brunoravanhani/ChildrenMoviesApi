using ChildrenMoviesApi.Domain.Entity;

namespace ChildrenMoviesApi.Domain.Interfaces;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetAll();
    Task<Movie> Get(int id);
    Task<bool> Save(Movie movie);
}