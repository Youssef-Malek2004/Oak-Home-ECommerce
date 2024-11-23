using Users.Domain.Entities;

namespace Users.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task AddUserAsync(User user);

    Task RemoveUserAsync(User user);

    Task EditUserAsync(User user);
}