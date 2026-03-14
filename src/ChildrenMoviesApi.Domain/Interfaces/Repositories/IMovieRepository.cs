using ChildrenMoviesApi.Domain.Entity;

namespace ChildrenMoviesApi.Domain.Interfaces.Repositories;

public interface IMovieRepository : IRepository
{
    Task<Movie?> GetByIdAsync(int id);
    Task<IEnumerable<Movie>> GetAllAsync();
    Task AddAsync(Movie movie);
    Task DeleteAsync(Movie movie);
}