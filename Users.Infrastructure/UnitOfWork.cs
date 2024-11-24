using Users.Domain;
using Users.Domain.Repositories;
using Users.Infrastructure.Persistence;
using Users.Infrastructure.Persistence.Repositories;

namespace Users.Infrastructure;

public class UnitOfWork(UsersDbContext dbContext) : IUnitOfWork
{
    private IUserRepository? _userRepository;

    public IUserRepository UserRepository => _userRepository ??= new UserRepository(dbContext);

    public async Task<int> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        dbContext.Dispose();
    }
}