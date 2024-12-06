using Abstractions.ResultsPattern;
using MediatR;
using Orders.Application.CQRS.Queries;
using Orders.Domain.DTOs.Orders;
using Orders.Domain.Repositories;

namespace Orders.Application.CQRS.QueryHandlers;

public class GetAllOrdersQueryHandler(IOrdersRepository repository)
    : IRequestHandler<GetAllOrdersQuery, Result<List<OrderResponse>>>
{
    public async Task<Result<List<OrderResponse>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var ordersResult = await repository.GetOrdersAsync(cancellationToken);
        if (!ordersResult.IsSuccess)
            return Result<List<OrderResponse>>.Failure(ordersResult.Error);

        var orders = ordersResult.Value!
            .Select(o => new OrderResponse
            {
                OrderId = o.OrderId,
                UserId = o.UserId.ToString(),
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                ShippingId = o.ShippingId,
                PaymentId = o.PaymentId,
                OrderItems = o.OrderItems.Select(oi => new OrderItemResponse
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    Subtotal = oi.Subtotal
                }).ToList()
            })
            .ToList();

        return Result<List<OrderResponse>>.Success(orders);
    }
}
