using Abstractions.ResultsPattern;
using MediatR;
using Users.Domain.DTOs;

namespace Users.Application.CQRS.Commands;

public class LoginCommand(LoginDto loginDto) : IRequest<Result<string>> //Returns JWT Token
{
    public LoginDto LoginDto { get; set; } = loginDto;
}