using Abstractions.ResultsPattern;
using Users.Domain.Entities;

namespace Users.Domain.Repositories;

public interface IUserRepository
{
    Task<Result<User?>> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<Result<User?>> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<Result> AddUserAsync(User user);

   Task<Result> RemoveUserAsync(User user);

    Task<Result> EditUserAsync(User user);
}