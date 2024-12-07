using Abstractions.ResultsPattern;
using Inventory.Application.CQRS.Commands;
using Inventory.Domain;
using Inventory.Domain.Errors;
using Inventory.Domain.Repositories;
using MediatR;

namespace Inventory.Application.CQRS.CommandHandlers;

public class SupplyInventoryHandler(IUnitOfWork unitOfWork) : IRequestHandler<SupplyInventoryCommand, Result>
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

        return Result.Success();
    }
}