using Abstractions.ResultsPattern;
using MediatR;
using Orders.Domain.DTOs.Orders;

namespace Orders.Application.CQRS.Commands;

public record CreateOrderCommand(CreateOrderRequest Request) : IRequest<Result<OrderResponse>>;