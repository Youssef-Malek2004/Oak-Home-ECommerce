using Microsoft.EntityFrameworkCore;
using Users.Application.Services;
using Users.Domain.Entities;
using Users.Domain.Repositories;

namespace Users.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository(IUsersDbContext dbContext) : IRefreshTokenRepository
{
    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
    }

    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        await dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
    }

    public async Task RevokeAsync(Guid tokenId, CancellationToken cancellationToken = default)
    {
        var token = await dbContext.RefreshTokens.FindAsync(new object[] { tokenId }, cancellationToken);
        if (token != null)
        {
            dbContext.RefreshTokens.Remove(token);
        }
    }
}