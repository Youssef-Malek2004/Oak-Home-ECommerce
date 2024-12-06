using Abstractions.ResultsPattern;
using MediatR;
using Orders.Application.CQRS.Commands;
using Orders.Domain;
using Orders.Domain.DTOs.Orders;
using Orders.Domain.Entities;
using Orders.Domain.Repositories;

namespace Orders.Application.CQRS.CommandHandlers;

public class CreateOrderCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateOrderCommand, Result<OrderResponse>>
{
    public async Task<Result<OrderResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            UserId = request.Request.UserId,
            OrderDate = request.Request.OrderDate,
            Status = request.Request.Status,
            TotalAmount = request.Request.TotalAmount,
            ShippingId = request.Request.ShippingId,
            PaymentId = request.Request.PaymentId,
            OrderItems = request.Request.OrderItems.Select(oi => new OrderItem
            {
                ProductId = oi.ProductId,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                Subtotal = oi.Quantity * oi.UnitPrice
            }).ToList()
        };

        var result = await unitOfWork.OrdersRepository.AddOrderAsync(order);
        if (!result.IsSuccess)
            return Result<OrderResponse>.Failure(result.Error);

        var response = new OrderResponse
        {
            OrderId = order.OrderId,
            UserId = order.UserId.ToString(),
            OrderDate = order.OrderDate,
            Status = order.Status,
            TotalAmount = order.TotalAmount,
            ShippingId = order.ShippingId,
            PaymentId = order.PaymentId,
            OrderItems = order.OrderItems.Select(oi => new OrderItemResponse
            {
                ProductId = oi.ProductId,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                Subtotal = oi.Subtotal
            }).ToList()
        };

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result<OrderResponse>.Success(response);
    }
}
