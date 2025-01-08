using Abstractions.ResultsPattern;
using MediatR;
using Payments.Application.CQRS.Commands;
using Payments.Domain.Entities;
using Payments.Domain.Repositories;
using Shared.Contracts.Events;
using Shared.Contracts.Events.PaymentEvents;
using Shared.Contracts.Kafka;
using Shared.Contracts.Topics;

namespace Payments.Application.CQRS.CommandHandlers;

public class CheckPaymentHandler(IKafkaProducerService producerService, IPaymentRepository paymentRepository) : IRequestHandler<CheckPaymentCommand, Result>
{
    public async Task<Result> Handle(CheckPaymentCommand request, CancellationToken cancellationToken)
    {
        var inventoryReservedEvent = request.InventoryReservedEvent;

        var payment = new Payment
        {
            OrderId = Guid.Parse(inventoryReservedEvent.OrderId),
            UserId = Guid.Parse(inventoryReservedEvent.UserId),
            Amount = inventoryReservedEvent.TotalAmount,
            Currency = "USD",
            PaymentMethod = "Card",
            Status = "Successful",
            TransactionId = "notYet",
            PaymentTime = DateTime.UtcNow
        };

        var result = await paymentRepository.AddPaymentAsync(payment);
        if (result.IsFailure) return result;
        
        //Handle Some kind of payment Logic toDo using stripe 
        
        await producerService.SendMessageAsync(Topics.PaymentEvents.Name,
            new PaymentProcessedEvent
            {
                OrderId = request.InventoryReservedEvent.OrderId,
                PaymentId = payment.Id.ToString(),
                Amount = payment.Amount,
                ProcessedAt = DateTime.UtcNow
            },
            cancellationToken,
            Event.PaymentSuccessfulEvent.Name);

        return result;
    }
}