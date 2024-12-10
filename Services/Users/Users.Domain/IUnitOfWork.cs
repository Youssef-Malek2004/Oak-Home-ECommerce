using Users.Domain.Repositories;

namespace Users.Domain;

public interface IUnitOfWork : IDisposable
{
    IRefreshTokenRepository RefreshTokenRepository { get; }
    IUserRepository UserRepository { get; }
    Task<int> SaveChangesAsync();
}