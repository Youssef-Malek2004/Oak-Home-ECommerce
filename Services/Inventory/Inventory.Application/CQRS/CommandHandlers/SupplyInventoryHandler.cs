using Abstractions.ResultsPattern;
using Inventory.Application.CQRS.Commands;
using Inventory.Domain;
using Inventory.Domain.Errors;
using MediatR;
using Shared.Contracts.Entities.NotificationService;
using Shared.Contracts.Events;
using Shared.Contracts.Kafka;
using Shared.Contracts.Topics;

namespace Inventory.Application.CQRS.CommandHandlers;

public class SupplyInventoryHandler(IUnitOfWork unitOfWork, IKafkaProducerService producerService) : IRequestHandler<SupplyInventoryCommand, Result>
{
    public async Task<Result> Handle(SupplyInventoryCommand request, CancellationToken cancellationToken)
    {
        var inventoryId = request.SupplyInventoryDto.InventoryId;

        var result = await unitOfWork.InventoryRepository.GetInventoryByIdAsync(inventoryId, cancellationToken);

        if (result.IsFailure) return result;

        var inventory = result.Value;

        if (inventory is null) return Result.Failure(InventoryErrors.InventoryNotFoundId(inventoryId));
        if (request.SupplyInventoryDto.Quantity <= 0)
            return Result.Failure(InventoryErrors.FailedToSupply(inventoryId));
        
        inventory.Quantity += request.SupplyInventoryDto.Quantity;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var notification = new Notification
        {
            Channel = Channels.WebSocket.Name,
            CreatedAt = DateTime.UtcNow,
            Group = Groups.None.Name,
            IsDelivered = false,
            IsRead = false,
            SentAt = DateTime.UtcNow,
            Type = Types.Success.Name,
            Title = "Inventory Supply",
            UserId = Guid.Parse(inventory.VendorId),
            Message = $"Inventory: {request.SupplyInventoryDto.InventoryId} " +
                      $"Supplied Successfully with Quantity: {request.SupplyInventoryDto.Quantity}"
        };

        var notificationRequest = new NotificationRequest { Notification = notification };

        await producerService.SendMessageAsync(Topics.NotificationRequests.Name,
            notificationRequest, cancellationToken, "");

        return Result.Success();
    }
}