using Abstractions.ResultsPattern;
using MediatR;
using Users.Domain.DTOs;

namespace Users.Application.CQRS.Commands;

public class ChangePasswordCommand(Guid userId, ChangePasswordDto changePasswordDto) : IRequest<Result>
{
    public Guid UserId { get; set; } = userId;

    public ChangePasswordDto ChangePasswordDto { get; set; } = changePasswordDto;
}