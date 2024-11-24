using Abstractions.ResultsPattern;
using MediatR;
using Users.Application.CQRS.Commands;
using Users.Application.Services;
using Users.Domain;
using Users.Domain.Errors;

namespace Users.Infrastructure.CQRS.CommandHandlers;

public class LoginCommandHandler(IUnitOfWork unitOfWork, IJwtProvider jwtProvider) : IRequestHandler<LoginCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.UserRepository.GetUserByEmailAsync(request.LoginDto.Email, cancellationToken);

        if (result.IsFailure)
        {
            return Result<string>.Failure(result.Error);
        }

        var user = result.Value;
        
        if (!BCrypt.Net.BCrypt.Verify(request.LoginDto.PasswordHash, user?.PasswordHash))
        {
            return Result<string>.Failure(UserErrors.InvalidCredentials);
        }

        var token = jwtProvider.Generate(user);

        return Result<string>.Success(token);
    }
}