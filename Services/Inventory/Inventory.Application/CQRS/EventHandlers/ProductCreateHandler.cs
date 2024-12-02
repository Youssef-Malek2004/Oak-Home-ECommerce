using Abstractions.ResultsPattern;
using Inventory.Application.CQRS.Events;
using Inventory.Domain;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.CQRS.EventHandlers;

public class ProductCreatedHandler(IUnitOfWork unitOfWork) : IRequestHandler<ProductCreatedEvent, Result<Inventories>>
{
    public async Task<Result<Inventories>> Handle(ProductCreatedEvent request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Product Created Event Received: ProductId: " +
                          $"{request.ProductCreated.ProductId}, VendorId: {request.ProductCreated.VendorId}");

        var productCreated = request.ProductCreated;

        Inventories inventories = new Inventories
        {
            ProductId = productCreated.ProductId,
            VendorId = productCreated.VendorId,
            WarehouseId = Guid.Parse(productCreated.WarehouseId),
            Price = productCreated.Price
        };
        
        var result = await unitOfWork.InventoryRepository.AddInventoryAsync(inventories);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return result.IsFailure ?
            Result<Inventories>.Failure(result.Error) : Result<Inventories>.Success(inventories);
    }
}