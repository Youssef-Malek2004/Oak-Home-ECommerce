using Users.Domain;
using Users.Domain.Repositories;
using Users.Infrastructure.Persistence;
using Users.Infrastructure.Persistence.Repositories;
using static System.GC;

namespace Users.Infrastructure;

public class UnitOfWork(UsersDbContext dbContext) : IUnitOfWork
{
    private IUserRepository? _userRepository;

    private IRefreshTokenRepository? _refreshTokenRepository;
    public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository ??= new RefreshTokenRepository(dbContext);
    public IUserRepository UserRepository => _userRepository ??= new UserRepository(dbContext); 

    public async Task<int> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        // SuppressFinalize(this);
        dbContext.Dispose();
    }
}