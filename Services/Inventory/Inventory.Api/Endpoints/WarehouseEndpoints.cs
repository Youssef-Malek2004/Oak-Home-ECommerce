using System.Linq.Expressions;
using Inventory.Application.CQRS.Queries;
using Inventory.Domain;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Api.Endpoints;

public static class WarehouseEndpoints
{
    public static void MapWarehouseEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/warehouses");

        group.MapGet("/", async (IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
        {
            var result = await unitOfWork.WarehouseRepository.GetWarehousesAsync(cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        });

        group.MapGet("/{id:guid}", async (Guid id, IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
        {
            var result = await unitOfWork.WarehouseRepository.GetWarehouseByIdAsync(id, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });
        
        group.MapGet("/names", async (IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetWarehousesProductQuery(), cancellationToken);
            return result.IsSuccess ? Results.Ok(new { warehouses = result.Value}) : Results.NotFound(result.Error);
        });

        group.MapPost("/", async (Warehouse warehouse, IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
        {
            var result = await unitOfWork.WarehouseRepository.AddWarehouseAsync(warehouse);
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Results.Created($"/warehouses/{warehouse.WarehouseId}", warehouse);
        });

        group.MapPut("/{id:guid}", async (Guid id, Warehouse warehouse, IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
        {
            warehouse.WarehouseId = id;
            var result = await unitOfWork.WarehouseRepository.UpdateWarehouseAsync(warehouse);
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Results.Ok(warehouse);
        });

        group.MapDelete("/{id:guid}", async (Guid id, IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
        {
            var warehouseResult = await unitOfWork.WarehouseRepository.GetWarehouseByIdAsync(id, cancellationToken);
            if (!warehouseResult.IsSuccess || warehouseResult.Value is null)
                return Results.NotFound(warehouseResult.Error);

            var result = await unitOfWork.WarehouseRepository.RemoveWarehouseAsync(warehouseResult.Value);
            if (!result.IsSuccess)
                return Results.BadRequest(result.Error);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Results.NoContent();
        });

        group.MapGet("/product-warehouses/{productId}", async (string productId, IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
        {
            var result = await unitOfWork.WarehouseRepository.GetWarehousesSellingProductAsync(productId, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });
        
        group.MapGet("/product-warehouse/{productId}", async (string productId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetWarehouseForProductQuery(productId), cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });

        group.MapGet("/filter", async (string? name, string? city, string? country, IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
        {
            Expression<Func<Warehouse, bool>> filter = w =>
                (string.IsNullOrEmpty(name) || w.Name.Contains(name)) &&
                (string.IsNullOrEmpty(city) || w.Address.City == city) &&
                (string.IsNullOrEmpty(country) || w.Address.Country == country);

            var result = await unitOfWork.WarehouseRepository.GetWarehousesByConditionAsync(filter, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        });
    }
}
