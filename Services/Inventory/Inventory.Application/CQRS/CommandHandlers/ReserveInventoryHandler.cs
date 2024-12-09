using Abstractions.ResultsPattern;
using Inventory.Application.CQRS.Commands;
using Inventory.Application.Services;
using Inventory.Domain;
using Inventory.Domain.Errors;
using MediatR;
using Shared.Contracts.Events;
using Shared.Contracts.Events.InventoryEvents;
using Shared.Contracts.Kafka;
using Shared.Contracts.Topics;

namespace Inventory.Application.CQRS.CommandHandlers;

public class ReserveInventoryHandler(IUnitOfWork unitOfWork, IKafkaProducerService producerService) : IRequestHandler<ReserveInventoryCommand, Result>
{
    public async Task<Result> Handle(ReserveInventoryCommand request, CancellationToken cancellationToken)
    {
        var order = request.OrderCreatedEvent;
        var unavailableItems = new List<NotEnoughInventoryEvent.UnavailableItem>();
        var reservedItems = new List<InventoryReservedEvent.ReservedItem>();

        foreach (var item in order.Items)
        {
            var warehousesResult = await unitOfWork.WarehouseRepository.GetWarehousesSellingProductAsync(item.ProductId, cancellationToken);

            if (!warehousesResult.IsSuccess || !warehousesResult.Value.Any())
            {
                unavailableItems.Add(new NotEnoughInventoryEvent.UnavailableItem
                {
                    ProductId = item.ProductId,
                    RequestedQuantity = item.Quantity,
                    AvailableQuantity = 0
                });
                continue;
            }

            var warehouses = warehousesResult.Value.ToList();
            var selectedWarehouse = warehouses
                .SelectMany(w => w.Inventories.Where(i => i.ProductId == item.ProductId))
                .FirstOrDefault(i => i.Quantity >= item.Quantity);

            if (selectedWarehouse == null)
            {
                var maxAvailableQuantity = warehouses
                    .SelectMany(w => w.Inventories.Where(i => i.ProductId == item.ProductId))
                    .Max(i => i.Quantity);

                unavailableItems.Add(new NotEnoughInventoryEvent.UnavailableItem
                {
                    ProductId = item.ProductId,
                    RequestedQuantity = item.Quantity,
                    AvailableQuantity = maxAvailableQuantity
                });
                continue;
            }
            
            selectedWarehouse.Quantity -= item.Quantity;
            selectedWarehouse.LastUpdated = DateTime.UtcNow;

            var updateResult = await unitOfWork.InventoryRepository.UpdateInventoryAsync(selectedWarehouse);
            if (!updateResult.IsSuccess)
            {
                return Result.Failure(updateResult.Error);
            }

            reservedItems.Add(new InventoryReservedEvent.ReservedItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            });
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        if (unavailableItems.Any())
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
            ReservedItems = reservedItems
        };

        await producerService.SendMessageAsync(
            Topics.InventoryEvents.Name,
            reservedEvent,
            cancellationToken,
            Event.InventoryReserved.Name);

        return Result.Success();
    }
}
