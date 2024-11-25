using Abstractions.ResultsPattern;
using MediatR;
using Users.Domain.DTOs;
using Users.Domain.Entities;

namespace Users.Application.CQRS.Commands;

public class AddAdminCommand(AddAdminDto addAdminDto) : IRequest<Result<User>>
{
    public AddAdminDto AddAdminDto { get; set; } = addAdminDto;
}