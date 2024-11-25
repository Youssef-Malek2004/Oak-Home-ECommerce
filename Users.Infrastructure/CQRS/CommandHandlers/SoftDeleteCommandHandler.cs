using Abstractions.ResultsPattern;
using MediatR;
using Users.Application.CQRS.Commands;
using Users.Domain;
using Users.Domain.Errors;
using Users.Domain.Repositories;

namespace Users.Infrastructure.CQRS.CommandHandlers;

public class SoftDeleteCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<SoftDeleteCommand, Result>
{
    public async Task<Result> Handle(SoftDeleteCommand request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.UserRepository.GetUserByIdAsync(request.UserId, cancellationToken);

        if (result.IsFailure)
        {
            return Result<string>.Failure(result.Error);
        }

        var user = result.Value;
        
        if (user == null) return Result.Failure(UserErrors.DoesntExist);
        
        user.IsDeleted = true;
        
        await unitOfWork.UserRepository.EditUserAsync(user);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}