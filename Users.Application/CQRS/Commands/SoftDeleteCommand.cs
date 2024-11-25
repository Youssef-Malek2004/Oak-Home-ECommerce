using Abstractions.ResultsPattern;
using MediatR;

namespace Users.Application.CQRS.Commands;

public class SoftDeleteCommand(Guid userId) : IRequest<Result>
{
    public Guid UserId { get; set; } = userId;
}