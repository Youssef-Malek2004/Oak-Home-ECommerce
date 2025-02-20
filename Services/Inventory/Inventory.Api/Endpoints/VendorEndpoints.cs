using Inventory.Application.CQRS.Commands;
using Inventory.Application.CQRS.Commands.Async;
using Inventory.Domain.DTOs.InventoryDtos;
using Inventory.Infrastructure.Authentication;
using MediatR;
using Shared.Contracts.Authorization;

namespace Inventory.Api.Endpoints;

public static class VendorEndpoints
{
    public static void MapVendorEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/vendors");

        group.MapPost("/{id:guid}/supply-inventory-async", async (Guid id, SupplyInventoryDto supplyInventoryDto,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new SupplyInventoryAsyncCommand(id,supplyInventoryDto)
                , cancellationToken);
            return result.IsSuccess ? Results.Ok() : Results.BadRequest();
        }).HasPermission(Permissions.MustBeSameUser.Name);
        
        group.MapPost("/{id:guid}/supply-inventory", async (Guid id, SupplyInventoryDto supplyInventoryDto,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new SupplyInventoryCommand(id,supplyInventoryDto)
                , cancellationToken);
            return result.IsSuccess ? Results.Ok() : Results.BadRequest();
        }).HasPermission(Permissions.MustBeSameUser.Name);

        group.MapPost("/{id:guid}/change-warehouse", async (Guid id, ChangeWarehouseDto changeWarehouseDto,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new ChangeWarehouseCommand(changeWarehouseDto)
                , cancellationToken);
            return result.IsSuccess ? Results.Ok() : Results.BadRequest();
        }).HasPermission(Permissions.MustBeSameUser.Name);
    } 
}