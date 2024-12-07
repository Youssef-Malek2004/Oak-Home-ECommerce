using Inventory.Domain;
using Inventory.Domain.Entities;

namespace Inventory.Api.Endpoints;

public static class InventoryEndpoints
{
    public static void MapInventoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/inventory");

        group.MapGet("/", async (IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
        {
            var result = await unitOfWork.InventoryRepository.GetInventoriesAsync(cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        });

        group.MapGet("/{id:guid}", async (Guid id, IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
        {
            var result = await unitOfWork.InventoryRepository.GetInventoryByIdAsync(id, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });

        group.MapGet("/product/{productId}", async (string productId, IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
        {
            var result = await unitOfWork.InventoryRepository.GetInventoriesByProductIdAsync(productId, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });

        group.MapPost("/", async (Inventories inventory, IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
        {
            var result = await unitOfWork.InventoryRepository.AddInventoryAsync(inventory);
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Results.Created($"/inventory/{inventory.Id}", inventory);
        });

        group.MapPut("/{id:guid}", async (Guid id, Inventories inventory, IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
        {
            inventory.Id = id;
            var result = await unitOfWork.InventoryRepository.UpdateInventoryAsync(inventory);
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Results.Ok(inventory);
        });

        group.MapDelete("/{id:guid}", async (Guid id, IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
        {
            var inventoryResult = await unitOfWork.InventoryRepository.GetInventoryByIdAsync(id, cancellationToken);
            if (!inventoryResult.IsSuccess || inventoryResult.Value is null)
                return Results.NotFound(inventoryResult.Error);

            var result = await unitOfWork.InventoryRepository.RemoveInventoryAsync(inventoryResult.Value);
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Results.NoContent();
        });

        group.MapPost("/soft-delete/{id:guid}", async (Guid id, bool isDeleted, IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
        {
            var inventoryResult = await unitOfWork.InventoryRepository.GetInventoryByIdAsync(id, cancellationToken);
            if (!inventoryResult.IsSuccess || inventoryResult.Value is null)
                return Results.NotFound(inventoryResult.Error);

            var result = await unitOfWork.InventoryRepository.SetSoftDelete(inventoryResult.Value, isDeleted, cancellationToken);
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Results.Ok();
        });
    }
}
