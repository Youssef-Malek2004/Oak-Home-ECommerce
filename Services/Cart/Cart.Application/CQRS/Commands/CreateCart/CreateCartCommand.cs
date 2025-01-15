using Abstractions.ResultsPattern;
using MediatR;

namespace Cart.Application.CQRS.Commands.CreateCart;

public record CreateCartCommand(Guid UserId) : IRequest<Result<Domain.Entities.Cart>>;