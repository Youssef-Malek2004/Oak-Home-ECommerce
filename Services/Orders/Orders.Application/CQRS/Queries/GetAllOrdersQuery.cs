using Abstractions.ResultsPattern;
using MediatR;
using Orders.Domain.DTOs.Orders;

namespace Orders.Application.CQRS.Queries;

public record GetAllOrdersQuery : IRequest<Result<List<OrderResponse>>>;