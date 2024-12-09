using Abstractions.ResultsPattern;
using MediatR;
using Orders.Application.CQRS.Commands;
using Orders.Domain;
using Orders.Domain.DTOs.Orders;
using Orders.Domain.Mappers;
using Shared.Contracts.Events;
using Shared.Contracts.Kafka;
using Shared.Contracts.Topics;

namespace Orders.Application.CQRS.CommandHandlers;

public class CreateOrderCommandHandler(IUnitOfWork unitOfWork, IKafkaProducerService producerService)
    : IRequestHandler<CreateOrderCommand, Result<OrderResponse>>
{
    public async Task<Result<OrderResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = request.Request.ToOrder();
        
        var result = await unitOfWork.OrdersRepository.AddOrderAsync(order);
        if (!result.IsSuccess)
            return Result<OrderResponse>.Failure(result.Error);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var orderCreatedEvent = order.ToOrderCreatedEvent();
        await producerService.SendMessageAsync(Topics.OrderEvents.Name, orderCreatedEvent, cancellationToken, Event.OrderCreated.Name);
      
        var response = order.ToOrderResponse();

        return Result<OrderResponse>.Success(response);
    }
}