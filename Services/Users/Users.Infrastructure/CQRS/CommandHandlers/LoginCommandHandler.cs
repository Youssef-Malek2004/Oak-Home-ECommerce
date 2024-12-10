using Abstractions.ResultsPattern;
using MediatR;
using Microsoft.AspNetCore.Http;
using Users.Application.CQRS.Commands;
using Users.Application.Services;
using Users.Domain;
using Users.Domain.Entities;
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

        if (user is null) return Result<string>.Failure(UserErrors.UserNotFoundEmail("Error during Login"));
        
        var token = jwtProvider.Generate(user);
        
        var refreshToken = jwtProvider.GenerateRefreshToken();
        
        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshToken,
            ExpiresOn = DateTime.UtcNow.AddDays(7),
            User = user
        };

        await unitOfWork.RefreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);
        await unitOfWork.SaveChangesAsync();
        
        request.HttpContext.Response.Cookies.Append("auth_token", token, new CookieOptions
        {
            HttpOnly = false, 
            Secure = true,   
            SameSite = SameSiteMode.Strict, 
            Expires = DateTime.UtcNow.AddDays(7) 
        });
        
        request.HttpContext.Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7) // Refresh token expires in 7 days
        });

        return Result<string>.Success(token);
    }
}