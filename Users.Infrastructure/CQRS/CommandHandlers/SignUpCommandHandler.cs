using Abstractions.ResultsPattern;
using MediatR;
using Users.Application.CQRS.Commands;
using Users.Domain;
using Users.Domain.Entities;
using Users.Domain.Errors;

namespace Users.Infrastructure.CQRS.CommandHandlers;

public class SignUpCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<SignUpCommand, Result<User>>
{
    public async Task<Result<User>> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var userExists = await unitOfWork.UserRepository.GetUserByEmailAsync(request.SignUpDto.Email, cancellationToken);
        
        if (userExists.IsSuccess)
        {
            return Result<User>.Failure(UserErrors.UserAlreadyExists(request.SignUpDto.Email));
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.SignUpDto.PasswordHash);
        var user = new User
        {
            Username = request.SignUpDto.Username,
            Email = request.SignUpDto.Email,
            PasswordHash = hashedPassword,
            Role = "User"
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
