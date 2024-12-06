using System.Linq.Expressions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.CQRS.Commands;
using Orders.Application.CQRS.Queries;
using Orders.Domain.DTOs.Orders;
using Orders.Domain.Entities;
using Orders.Domain.Repositories;

namespace Orders.Api.Endpoints;

public static class OrdersEndpoints
{
    public static void MapOrdersCrudEndpoints(this IEndpointRouteBuilder app)
    {
        var ordersEndpoints = app.MapGroup("orders");

        ordersEndpoints.MapGet("", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllOrdersQuery());
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        });

        ordersEndpoints.MapGet("{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetOrderByIdQuery(id));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });

        ordersEndpoints.MapPost("", async ([FromBody] CreateOrderRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateOrderCommand(request));
            return result.IsSuccess ? Results.Created($"/orders/{result.Value!.OrderId}", result.Value) : Results.BadRequest(result.Error);
        });

        ordersEndpoints.MapPut("{id:guid}", async (Guid id, [FromBody] CreateOrderRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateOrderCommand(id, request));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        });

        ordersEndpoints.MapDelete("{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteOrderCommand(id));
            return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error);
        });
        ordersEndpoints.MapGet("filter", async (string? status, string? userId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetOrdersByConditionQuery(status, userId));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        });

    }
}