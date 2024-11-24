using Abstractions.ResultsPattern;
using MediatR;
using Users.Domain.DTOs;
using Users.Domain.Entities;

namespace Users.Application.CQRS.Commands;

public class SignUpCommand(SignUpDto signUpDto) : IRequest<Result<User>>
{
    public SignUpDto SignUpDto { get; set; } = signUpDto;
}
