using Abstractions.ResultsPattern;
using MediatR;
using Orders.Domain.DTOs.Orders;

namespace Orders.Application.CQRS.Queries;

public record GetOrdersByConditionQuery(string? Status, string? UserId) : IRequest<Result<List<OrderResponse>>>;
