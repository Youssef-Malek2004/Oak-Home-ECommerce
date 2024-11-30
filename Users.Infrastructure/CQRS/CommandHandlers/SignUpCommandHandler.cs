using Abstractions.ResultsPattern;
using MediatR;
using Users.Application.CQRS.Commands;
using Users.Domain;
using Users.Domain.Entities;
using Users.Domain.Errors;
using Users.Infrastructure.Persistence;

namespace Users.Infrastructure.CQRS.CommandHandlers;

public class SignUpCommandHandler(IUnitOfWork unitOfWork, UsersDbContext dbContext) : IRequestHandler<SignUpCommand, Result<User>>
{
    public async Task<Result<User>> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var userExists = await unitOfWork.UserRepository.GetUserByEmailAsync(request.SignUpDto.Email, cancellationToken);
        
        if (userExists.IsSuccess)
        {
            return Result<User>.Failure(UserErrors.UserAlreadyExists(request.SignUpDto.Email));
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.SignUpDto.PasswordHash);
        
        var registeredRole = await dbContext.Set<Role>().FindAsync(Role.Registered.Id);

        if (registeredRole is null) return Result<User>.Failure(new Error("RoleNotFound", "Registered Role Not found"));
        
        var roles = new List<Role>() { registeredRole };

        if (request.AccountType.Equals(Role.Admin))
        {
            var adminRole = await dbContext.Set<Role>().FindAsync(Role.Admin.Id);
            
            if (adminRole is null) return Result<User>.Failure(new Error("RoleNotFound", "Admin Role Not found"));
            
            roles.Add(adminRole);
        }

        if (request.AccountType.Equals(Role.Vendor))
        {
            var vendorRole = await dbContext.Set<Role>().FindAsync(Role.Vendor.Id);
            
            if (vendorRole is null) return Result<User>.Failure(new Error("RoleNotFound", "Vendor Role Not found"));
            
            roles.Add(vendorRole);
        }
        
        var user = new User
        {
            Username = request.SignUpDto.Username,
            Email = request.SignUpDto.Email,
            PasswordHash = hashedPassword,
            Roles = roles
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
