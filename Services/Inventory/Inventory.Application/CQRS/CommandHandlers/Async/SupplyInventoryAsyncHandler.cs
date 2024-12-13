using Abstractions.ResultsPattern;
using Inventory.Application.CQRS.Commands.Async;
using MediatR;
using Shared.Contracts.Entities.NotificationService;
using Shared.Contracts.Events;
using Shared.Contracts.Kafka;
using Shared.Contracts.Topics;

namespace Inventory.Application.CQRS.CommandHandlers.Async;

public class SupplyInventoryAsyncHandler(IKafkaProducerService kafkaProducerService,
    IKafkaNotificationService kafkaNotificationService) : IRequestHandler<SupplyInventoryAsyncCommand, Result>
{
    public async Task<Result> Handle(SupplyInventoryAsyncCommand request, CancellationToken cancellationToken)
    {
        await kafkaProducerService.SendMessageAsync(Topics.InventoryEvents.Name, request, cancellationToken,
            Event.InventorySupplyRequest.Name);

        var notification = NotificationsFactory.
            GenerateInfoWebNotificationUser(request.VendorId.ToString(), Groups.Vendors.Name,
                "Inventory Supplying", $"Inventory: {request.SupplyInventoryDto.InventoryId} supplying in progress.");

        await kafkaNotificationService.SendNotificationAsync(notification, cancellationToken);
        
        return Result.Success();
    }
}

