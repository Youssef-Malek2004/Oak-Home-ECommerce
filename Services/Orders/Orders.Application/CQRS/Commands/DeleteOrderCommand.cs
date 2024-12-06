using Abstractions.ResultsPattern;
using MediatR;

namespace Orders.Application.CQRS.Commands;


public record DeleteOrderCommand(Guid OrderId) : IRequest<Result>;