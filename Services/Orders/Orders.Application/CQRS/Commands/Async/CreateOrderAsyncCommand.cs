using Abstractions.ResultsPattern;
using MediatR;
using Orders.Domain.DTOs.Orders;

namespace Orders.Application.CQRS.Commands.Async;

public record CreateOrderAsyncCommand(CreateOrderRequest Request) : IRequest<Result<OrderResponse>>;
