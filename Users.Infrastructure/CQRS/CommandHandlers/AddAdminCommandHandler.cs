using Abstractions.ResultsPattern;
using MediatR;
using Users.Application.CQRS.Commands;
using Users.Application.Services;
using Users.Domain;
using Users.Domain.Entities;
using Users.Domain.Errors;
using Users.Infrastructure.Persistence;

namespace Users.Infrastructure.CQRS.CommandHandlers;

public class AddAdminCommandHandler(IUnitOfWork unitOfWork, UsersDbContext dbContext) : IRequestHandler<AddAdminCommand, Result<User>>
{
    public async Task<Result<User>> Handle(AddAdminCommand request, CancellationToken cancellationToken)
    {
        var userExists = await unitOfWork.UserRepository.GetUserByEmailAsync(request.AddAdminDto.Email, cancellationToken);
        
        if (userExists.IsSuccess)
        {
            return Result<User>.Failure(UserErrors.UserAlreadyExists(request.AddAdminDto.Email));
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.AddAdminDto.PasswordHash);

        var registeredRole = await dbContext.Set<Role>().FindAsync(Role.Admin.Id);

        if (registeredRole == null)
        {
            return Result<User>.Failure(new Error("RoleNotFound", "Registered role not found."));
        }
        
        var user = new User
        {
            Username = request.AddAdminDto.Username,
            Email = request.AddAdminDto.Email,
            PasswordHash = hashedPassword,
            Roles = new List<Role>() { registeredRole }
        };

        var result = await unitOfWork.UserRepository.AddUserAsync(user);
        
        if (result.IsFailure)
        {
            return Result<User>.Failure(result.Error);
        }
            
        await unitOfWork.SaveChangesAsync();
        return Result<User>.Success(user);
    }
}