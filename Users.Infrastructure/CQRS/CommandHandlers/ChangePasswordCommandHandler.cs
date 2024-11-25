using Abstractions.ResultsPattern;
using MediatR;
using Users.Application.CQRS.Commands;
using Users.Domain;
using Users.Domain.Entities;
using Users.Domain.Errors;

namespace Users.Infrastructure.CQRS.CommandHandlers;

public class ChangePasswordCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ChangePasswordCommand, Result>
{
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.UserRepository.GetUserByIdAsync(request.UserId, cancellationToken);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        var user = result.Value;
        
        if (!BCrypt.Net.BCrypt.Verify(request.ChangePasswordDto.OldPassword, user?.PasswordHash))
        {
            return Result.Failure(UserErrors.InvalidCredentials);
        }
        
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.ChangePasswordDto.NewPassword);

        if (user is null) return Result.Failure(UserErrors.DoesntExist);
        
        user.PasswordHash = hashedPassword;

        await unitOfWork.UserRepository.EditUserAsync(user);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}