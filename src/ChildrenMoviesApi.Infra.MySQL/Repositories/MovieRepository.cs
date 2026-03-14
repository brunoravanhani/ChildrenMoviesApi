using ChildrenMoviesApi.Domain.Entity;
using ChildrenMoviesApi.Domain.Interfaces.Repositories;
using ChildrenMoviesApi.Infra.MySQL.Data;
using Microsoft.EntityFrameworkCore;

namespace ChildrenMoviesApi.Infra.MySQL.Repositories;

internal class MovieRepository : IMovieRepository
{
    private readonly ChildrenMoviesDbContext _context;

    public MovieRepository(ChildrenMoviesDbContext context)
    {
        _context = context;
    }

    public async Task<Movie?> GetByIdAsync(int id)
    {
        return await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        return await _context.Movies.ToListAsync();
    }

    public async Task AddAsync(Movie movie)
    {
        await _context.Movies.AddAsync(movie);
    }

    public async Task DeleteAsync(Movie movie)
    {
        _context.Movies.Remove(movie);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}