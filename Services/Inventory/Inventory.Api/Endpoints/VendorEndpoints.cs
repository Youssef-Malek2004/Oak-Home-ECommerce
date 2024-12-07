using Inventory.Application.CQRS.Commands;
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

        group.MapPost("/{id:guid}/supply-inventory", async (Guid id, SupplyInventoryDto supplyInventoryDto,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new SupplyInventoryCommand(supplyInventoryDto)
                , cancellationToken);
            return result.IsSuccess ? Results.Ok() : Results.BadRequest();
        }).HasPermission(Permissions.MustBeSameUser.Name);
    } 
}