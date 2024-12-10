using Abstractions.ResultsPattern;
using MediatR;
using Microsoft.AspNetCore.Http;
using Users.Application.CQRS.Commands;
using Users.Application.Services;
using Users.Domain;
using Users.Domain.Errors;

namespace Users.Infrastructure.CQRS.CommandHandlers;

public class RefreshTokenHandler(IUnitOfWork unitOfWork, IJwtProvider jwtProvider) : IRequestHandler<RefreshTokenCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = request.HttpContext.Request.Cookies["refresh_token"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Result<string>.Failure(RefreshTokenErrors.RefreshTokenMissing());
            }
            
            var storedToken = await unitOfWork.RefreshTokenRepository.GetByTokenAsync(refreshToken, cancellationToken);

            if (storedToken == null || storedToken.ExpiresOn <= DateTime.UtcNow)
            {
                return Result<string>.Failure(RefreshTokenErrors.RefreshTokenInvalidOrExpired());
            }
            
            var result = await unitOfWork.UserRepository.GetUserByIdAsync(storedToken.UserId, cancellationToken);

            if (result.IsFailure)
            {
                return Result<string>.Failure(result.Error);
            }

            var user = result.Value;

            if (user is null) return Result<string>.Failure(UserErrors.UserNotFoundId(storedToken.UserId));
            
            var newJwtToken = jwtProvider.Generate(user);
            
            storedToken.Token = jwtProvider.GenerateRefreshToken();
            storedToken.ExpiresOn = DateTime.UtcNow.AddDays(7);
            await unitOfWork.SaveChangesAsync();
            
            request.HttpContext.Response.Cookies.Append("auth_token", newJwtToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7) 
            });
            
            request.HttpContext.Response.Cookies.Append("refresh_token", storedToken.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = storedToken.ExpiresOn
            });

            return Result<string>.Success(newJwtToken);
        }
    }