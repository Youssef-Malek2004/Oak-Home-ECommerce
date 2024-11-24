using Abstractions.ResultsPattern;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;
using Users.Domain.Errors;
using Users.Domain.Repositories;

namespace Users.Infrastructure.Persistence.Repositories;

public class UserRepository(UsersDbContext dbContext) : IUserRepository
{
    public async Task<Result<User?>> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        if (user == null)
        {
            return Result<User?>.Failure(UserErrors.UserNotFoundId(id));
        }

        return Result<User?>.Success(user);
    }

    public async Task<Result<User?>> GetUserByEmailAsync(string email , CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        
        if (user == null)
        {
            return Result<User?>.Failure(UserErrors.UserNotFoundEmail(email));
        }

        return Result<User?>.Success(user);
    }

    public async Task<Result> AddUserAsync(User user)
    {
        try
        {
            await dbContext.Users.AddAsync(user);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return UserErrors.UserAddFailed(ex.Message);
        }
    }

    public async Task<Result> RemoveUserAsync(User user)
    {
        try
        {
            dbContext.Users.Remove(user);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return UserErrors.UserRemoveFailed(ex.Message);
        }
    }

    public async Task<Result> EditUserAsync(User user)
    {
        try
        {
            dbContext.Users.Update(user);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return UserErrors.UserEditFailed(ex.Message);
        }
    }
}