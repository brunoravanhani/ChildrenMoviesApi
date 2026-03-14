using ChildrenMoviesApi.Domain.Entity;
using ChildrenMoviesApi.Domain.Interfaces.Repositories;
using ChildrenMoviesApi.Infra.MySQL.Data;
using Microsoft.EntityFrameworkCore;

namespace ChildrenMoviesApi.Infra.MySQL.Repositories;

internal class UserMovieRepository : IUserMovieRepository
{
    private readonly ChildrenMoviesDbContext _context;

    public UserMovieRepository(ChildrenMoviesDbContext context)
    {
        _context = context;
    }

    public async Task<UserMovie?> GetByIdAsync(Guid id)
    {
        return await _context.UserMovies.FirstOrDefaultAsync(um => um.Id == id);
    }

    public async Task<IEnumerable<UserMovie>> GetAllByUserAsync(string userId)
    {
        return await _context.UserMovies.Include(u => u.Movie).Where(u => u.UserId == userId).ToListAsync();
    }

    public async Task AddAsync(UserMovie userMovie)
    {
        await _context.UserMovies.AddAsync(userMovie);
    }

    public async Task DeleteAsync(UserMovie userMovie)
    {
        _context.UserMovies.Remove(userMovie);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
