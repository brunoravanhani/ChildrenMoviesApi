using ChildrenMoviesApi.Domain.Entity;

namespace ChildrenMoviesApi.Domain.Interfaces.Repositories;

public interface IUserMovieRepository : IRepository
{
    Task<UserMovie?> GetByIdAsync(Guid id);
    Task<IEnumerable<UserMovie>> GetAllByUserAsync(string userId);
    Task<UserMovie?> GetByMovieAndUserAsync(int movieId, string userId);
    Task AddAsync(UserMovie userMovie);
    Task DeleteAsync(UserMovie userMovie);
}
