using ChildrenMoviesApi.Domain.Entity;

namespace ChildrenMoviesApi.Domain.Interfaces;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetAll();
    Task<Movie> Get(Guid id);
    Task<bool> Save(Movie movie);
}