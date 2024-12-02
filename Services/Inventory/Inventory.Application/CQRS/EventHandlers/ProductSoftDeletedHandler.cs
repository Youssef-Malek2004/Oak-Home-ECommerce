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

        var inventory = result.Value;
        
        if (result.IsFailure | inventory is null)
            return Result.Failure(InventoryErrors.InventoryNotFoundProductId(request.ProductSoftDeleted.ProductId));
        

        await unitOfWork.InventoryRepository.SetSoftDelete(inventory!, request.ProductSoftDeleted.IsDeleted,
            cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}