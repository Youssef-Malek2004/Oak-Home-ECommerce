using Abstractions.ResultsPattern;
using MediatR;
using Orders.Domain.DTOs.Orders;

namespace Orders.Application.CQRS.Commands;

public record UpdateOrderCommand(Guid OrderId, CreateOrderRequest Request) : IRequest<Result<OrderResponse>>;