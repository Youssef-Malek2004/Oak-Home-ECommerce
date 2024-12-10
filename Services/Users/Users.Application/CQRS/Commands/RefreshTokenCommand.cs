using Abstractions.ResultsPattern;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Users.Application.CQRS.Commands;

public record RefreshTokenCommand(HttpContext HttpContext) : IRequest<Result<string>>;