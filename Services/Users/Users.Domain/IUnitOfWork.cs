using Users.Domain.Repositories;

namespace Users.Domain;

public interface IUnitOfWork : IDisposable
{
    IUserRepository UserRepository { get; }
    Task<int> SaveChangesAsync();
}