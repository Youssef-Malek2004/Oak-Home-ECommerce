using Abstractions.ResultsPattern;
using Inventory.Application.CQRS.Commands;
using Inventory.Domain;
using Inventory.Domain.Errors;
using MediatR;

namespace Inventory.Application.CQRS.CommandHandlers;

public class ChangeWarehouseHandler(IUnitOfWork unitOfWork) : IRequestHandler<ChangeWarehouseCommand, Result>
{
    public async Task<Result> Handle(ChangeWarehouseCommand request, CancellationToken cancellationToken)
    {
        var inventoryId = request.ChangeWarehouseDto.InventoryId;
        var warehouseId = request.ChangeWarehouseDto.WarehouseId;

        var inventoryResult = await unitOfWork.InventoryRepository.
            GetInventoryByIdAsync(inventoryId, cancellationToken);

        if (inventoryResult.IsFailure) return inventoryResult;

        var inventory = inventoryResult.Value;
        
        if (inventory is null) return Result.Failure(InventoryErrors.InventoryNotFoundId(inventoryId));
        
        if (warehouseId == inventory.WarehouseId) return Result.Success();

        var pastWarehouseResult =
            await unitOfWork.WarehouseRepository.GetWarehouseByIdAsync(inventory.WarehouseId, cancellationToken);
        
        var newWarehouseResult =
            await unitOfWork.WarehouseRepository.GetWarehouseByIdAsync(warehouseId, cancellationToken);

        if (pastWarehouseResult.IsFailure) return pastWarehouseResult;
        if (newWarehouseResult.IsFailure) return newWarehouseResult;

        var pastWarehouse = pastWarehouseResult.Value;
        var newWarehouse = newWarehouseResult.Value;

        if (pastWarehouse is null) return Result.Failure(WarehouseErrors.WarehouseNotFound(inventory.WarehouseId));
        if (newWarehouse is null) return Result.Failure(WarehouseErrors.WarehouseNotFound(warehouseId));
        
        await unitOfWork.WarehouseRepository.RemoveInventoryFromWarehouse(pastWarehouse, inventory);
        await unitOfWork.WarehouseRepository.AddInventoryToWarehouse(newWarehouse, inventory);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}