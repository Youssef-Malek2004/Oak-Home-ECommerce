using System.Linq.Expressions;
using Abstractions.ResultsPattern;
using MediatR;
using Orders.Application.CQRS.Queries;
using Orders.Domain.DTOs.Orders;
using Orders.Domain.Entities;
using Orders.Domain.Mappers;
using Orders.Domain.Repositories;

namespace Orders.Application.CQRS.QueryHandlers;

public class GetOrdersByConditionQueryHandler(IOrdersRepository repository)
    : IRequestHandler<GetOrdersByConditionQuery, Result<List<OrderResponse>>>
{
    public async Task<Result<List<OrderResponse>>> Handle(GetOrdersByConditionQuery request, CancellationToken cancellationToken)
    {
        var predicate = BuildFilterPredicate(request.Status, request.UserId);
        var ordersResult = await repository.GetOrdersByConditionAsync(predicate, cancellationToken);

        if (!ordersResult.IsSuccess)
        {
            return Result<List<OrderResponse>>.Failure(ordersResult.Error);
        }

        var orders = ordersResult.Value!.Select(o => o.ToOrderResponse()).ToList();
        return Result<List<OrderResponse>>.Success(orders);
    }

    private static Expression<Func<Order, bool>> BuildFilterPredicate(string? status, string? userId)
    {
        return o =>
            (string.IsNullOrEmpty(status) || o.Status == status) &&
            (string.IsNullOrEmpty(userId) || o.UserId.ToString() == userId);
    }
}
