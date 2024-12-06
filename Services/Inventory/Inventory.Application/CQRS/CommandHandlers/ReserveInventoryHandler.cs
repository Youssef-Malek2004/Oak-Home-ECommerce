using Abstractions.ResultsPattern;
using Inventory.Application.CQRS.Commands;
using Inventory.Application.Services;
using Inventory.Domain;
using Inventory.Domain.Errors;
using MediatR;
using Shared.Contracts.Events;
using Shared.Contracts.Events.InventoryEvents;
using Shared.Contracts.Topics;

namespace Inventory.Application.CQRS.CommandHandlers;

public class ReserveInventoryHandler(IUnitOfWork unitOfWork, IKafkaProducerService producerService) : IRequestHandler<ReserveInventoryCommand, Result>
{
     public async Task<Result> Handle(ReserveInventoryCommand request, CancellationToken cancellationToken)
    {
        var order = request.OrderCreatedEvent;
        var unavailableItems = new List<NotEnoughInventoryEvent.UnavailableItem>();

        foreach (var item in order.Items)
        {
            var inventoryResult = await unitOfWork.InventoryRepository.GetInventoriesByProductIdAsync(item.ProductId, cancellationToken);
            
            if (!inventoryResult.IsSuccess)
            {
                unavailableItems.Add(new NotEnoughInventoryEvent.UnavailableItem
                {
                    ProductId = item.ProductId,
                    RequestedQuantity = item.Quantity,
                    AvailableQuantity = 0
                });
                continue;
            }

            var inventory = inventoryResult.Value;
            
            if (inventory == null || inventory.Quantity < item.Quantity)
            {
                unavailableItems.Add(new NotEnoughInventoryEvent.UnavailableItem
                {
                    ProductId = item.ProductId,
                    RequestedQuantity = item.Quantity,
                    AvailableQuantity = inventory!.Quantity
                });
                continue;
            }

            inventory.Quantity -= item.Quantity;
            inventory.LastUpdated = DateTime.UtcNow;

            var updateResult = await unitOfWork.InventoryRepository.UpdateInventoryAsync(inventory);
            if (!updateResult.IsSuccess)
            {
                return Result.Failure(updateResult.Error);
            }
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        if (unavailableItems.Count != 0)
        {
            var notEnoughEvent = new NotEnoughInventoryEvent
            {
                OrderId = order.OrderId,
                UnavailableItems = unavailableItems
            };

            await producerService.SendMessageAsync(
                Topics.InventoryEvents.Name,
                notEnoughEvent,
                cancellationToken,
                Event.InventoryNotEnough.Name);

            return Result.Failure(InventoryErrors.InventoryNotEnough("Inventory not sufficient for some items."));
        }
        
        var reservedEvent = new InventoryReservedEvent
        {
            OrderId = order.OrderId,
            ReservedItems = order.Items.Select(item => new InventoryReservedEvent.ReservedItem()
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            }).ToList()
        };

        await producerService.SendMessageAsync(
            Topics.InventoryEvents.Name,
            reservedEvent,
            cancellationToken,
            Event.InventoryReserved.Name);

        return Result.Success();
    }
}