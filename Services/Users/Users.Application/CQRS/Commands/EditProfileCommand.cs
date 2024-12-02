using Abstractions.ResultsPattern;
using MediatR;
using Users.Domain.DTOs;

namespace Users.Application.CQRS.Commands;

public class EditProfileCommand(Guid userId, EditProfileDto editProfileDto) : IRequest<Result>
{
    public Guid UserId { get; set; } = userId;
    public EditProfileDto EditProfileDto { get; set; } = editProfileDto;
}