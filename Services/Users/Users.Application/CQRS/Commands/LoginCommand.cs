using Abstractions.ResultsPattern;
using MediatR;
using Microsoft.AspNetCore.Http;
using Users.Domain.DTOs;

namespace Users.Application.CQRS.Commands;

public class LoginCommand(LoginDto loginDto, HttpContext httpContext) : IRequest<Result<string>> //Returns JWT Token
{
    public LoginDto LoginDto { get; set; } = loginDto;
    public HttpContext HttpContext { get; set; } = httpContext;
}