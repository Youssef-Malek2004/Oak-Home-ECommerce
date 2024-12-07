using Abstractions.ResultsPattern;
using Inventory.Application.CQRS.Events;
using Inventory.Domain;
using Inventory.Domain.Errors;
using MediatR;

namespace Inventory.Application.CQRS.EventHandlers;

public class ProductSoftDeletedHandler(IUnitOfWork unitOfWork) : IRequestHandler<ProductSoftDeletedEvent, Result>
{
    public async Task<Result> Handle(ProductSoftDeletedEvent request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Product Soft-Deleted Event Received: ProductId: " +
                          $"{request.ProductSoftDeleted.ProductId}, Soft-Deletion-Value: {request.ProductSoftDeleted.IsDeleted}");

        var result = await unitOfWork.InventoryRepository
            .GetInventoriesByProductIdAsync(request.ProductSoftDeleted.ProductId, cancellationToken);

        if (result.IsFailure || (result.Value != null && !result.Value.Any()))
        {
            return Result.Failure(InventoryErrors.InventoryNotFoundProductId(request.ProductSoftDeleted.ProductId));
        }

        var inventories = result.Value;

        foreach (var inventory in inventories!)
        {
            var setSoftDeleteResult = await unitOfWork.InventoryRepository.SetSoftDelete(inventory, request.ProductSoftDeleted.IsDeleted, cancellationToken);
            if (setSoftDeleteResult.IsFailure)
            {
                return Result.Failure(setSoftDeleteResult.Error);
            }
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}