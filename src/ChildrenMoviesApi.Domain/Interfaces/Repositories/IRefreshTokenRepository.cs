using ChildrenMoviesApi.Domain.Entity;

namespace ChildrenMoviesApi.Domain.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetValidTokenAsync(string token);
    Task RevokeAsync(string token);
    Task<bool> SaveChangesAsync();
}
