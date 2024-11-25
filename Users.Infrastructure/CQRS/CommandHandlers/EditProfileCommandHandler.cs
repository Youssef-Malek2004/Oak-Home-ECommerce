using Abstractions.ResultsPattern;
using MediatR;
using Users.Application.CQRS.Commands;
using Users.Domain;
using Users.Domain.Errors;

namespace Users.Infrastructure.CQRS.CommandHandlers;

public class EditProfileCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<EditProfileCommand, Result>
{
    public async Task<Result> Handle(EditProfileCommand request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.UserRepository.GetUserByIdAsync(request.UserId, cancellationToken);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        var user = result.Value;

        if (user is null) return Result.Failure(UserErrors.DoesntExist);
        
        user.Username = request.EditProfileDto.Username;

        await unitOfWork.UserRepository.EditUserAsync(user);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}