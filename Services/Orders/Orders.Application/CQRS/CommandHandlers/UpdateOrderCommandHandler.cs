using Abstractions.ResultsPattern;
using MediatR;
using Orders.Application.CQRS.Commands;
using Orders.Application.Services.Data;
using Orders.Domain;
using Orders.Domain.DTOs.Orders;
using Orders.Domain.Errors;
using Orders.Domain.Mappers;

namespace Orders.Application.CQRS.CommandHandlers;

public class UpdateOrderCommandHandler(IUnitOfWork unitOfWork, IOrdersDbContext dbContext)
    : IRequestHandler<UpdateOrderCommand, Result<OrderResponse>>
{
    public async Task<Result<OrderResponse>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        
        var existingOrderResult = await unitOfWork.OrdersRepository.GetOrderByIdAsync(request.OrderId, cancellationToken);
        if (!existingOrderResult.IsSuccess || existingOrderResult.Value == null)
        {
            return Result<OrderResponse>.Failure(existingOrderResult.Error);
        }

        var orderToUpdate = existingOrderResult.Value;
        
        orderToUpdate.UserId = request.Request.UserId;
        orderToUpdate.OrderDate = request.Request.OrderDate;
        orderToUpdate.Status = request.Request.Status;
        orderToUpdate.TotalAmount = request.Request.TotalAmount;
        orderToUpdate.ShippingId = request.Request.ShippingId;
        orderToUpdate.PaymentId = request.Request.PaymentId;
        
        var existingOrderItems = orderToUpdate.OrderItems.ToList();

        foreach (var requestItem in request.Request.OrderItems)
        {
            var existingItem = existingOrderItems.FirstOrDefault(oi => oi.ProductId == requestItem.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity = requestItem.Quantity;
                existingItem.UnitPrice = requestItem.UnitPrice;
                existingItem.Subtotal = requestItem.Quantity * requestItem.UnitPrice;
            }
            else
            {
                var newItem = requestItem.ToOrderItem();
                newItem.OrderId = orderToUpdate.OrderId;
                orderToUpdate.OrderItems.Add(newItem);
                await dbContext.OrderItems.AddAsync(newItem, cancellationToken);
            }
        }
        
        foreach (var existingItem in existingOrderItems)
        {
            if (request.Request.OrderItems.All(oi => oi.ProductId != existingItem.ProductId))
            {
                orderToUpdate.OrderItems.Remove(existingItem);
            }
        }
        
        var updateResult = await unitOfWork.OrdersRepository.UpdateOrderAsync(orderToUpdate);
        
        if (!updateResult.IsSuccess)
        {
            return Result<OrderResponse>.Failure(updateResult.Error);
        }

        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<OrderResponse>.Failure(OrderErrors.OrderUpdateFailed(ex.Message));
        }
        
        return Result<OrderResponse>.Success(orderToUpdate.ToOrderResponse());
    }
}
