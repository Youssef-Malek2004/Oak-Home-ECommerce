using Abstractions.ResultsPattern;
using MediatR;
using Orders.Application.CQRS.Commands;
using Orders.Domain;
using Orders.Domain.Entities;
using Orders.Domain.Errors;

namespace Orders.Application.CQRS.CommandHandlers;

public class FailedOrderHandler(IUnitOfWork unitOfWork) : IRequestHandler<FailedOrderCommand, Result>
{
    public async Task<Result> Handle(FailedOrderCommand request, CancellationToken cancellationToken)
    {
        var orderId = Guid.Parse(request.NotEnoughInventoryEvent.OrderId);
        
        var orderResult = await unitOfWork.OrdersRepository.GetOrderByIdAsync(orderId, cancellationToken);

        if (!orderResult.IsSuccess || orderResult.Value == null)
        {
            return Result.Failure(OrderErrors.OrderNotFound(orderId));
        }
        
        var order = orderResult.Value;
        
        order.Status = "Failed";
        
        var updateResult = await unitOfWork.OrdersRepository.UpdateOrderAsync(order);

        if (!updateResult.IsSuccess)
        {
            return Result.Failure(OrderErrors.OrderUpdateFailed($"Update failed for order: {orderId}"));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
        
    }
}