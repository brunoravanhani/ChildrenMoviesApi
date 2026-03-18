using ChildrenMoviesApi.Domain.Entity;
using ChildrenMoviesApi.Domain.Interfaces.Repositories;
using ChildrenMoviesApi.Infra.MySQL.Data;
using Microsoft.EntityFrameworkCore;

namespace ChildrenMoviesApi.Infra.MySQL.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ChildrenMoviesDbContext _context;

    public RefreshTokenRepository(ChildrenMoviesDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
    }

    public async Task<RefreshToken?> GetValidTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token && rt.IsActive);
    }

    public async Task RevokeAsync(string token)
    {
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token);

        if (refreshToken != null)
        {
            refreshToken.RevokedDate = DateTime.UtcNow;
        }
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
