using Cart.Application.CQRS.Commands;
using Cart.Application.CQRS.Queries;
using Cart.Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Contracts.Authorization;

namespace Cart.Api.Endpoints;

public static class CartEndpoints
{
    public static void MapCartEndpoints(this IEndpointRouteBuilder app)
    {
        var cartEndpoints = app.MapGroup("cart").RequireAuthorization();

        cartEndpoints.MapGet("{userId:guid}", async (
            Guid userId,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetCartQuery(userId), cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        }).HasPermission(Permissions.MustBeSameUser.ToString());

        cartEndpoints.MapPost("{userId:guid}", async (
            Guid userId,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new CreateCartCommand(userId), cancellationToken);
            return result.IsSuccess
                ? Results.Created($"/cart/{userId}", result.Value)
                : Results.BadRequest(result.Error);
        }).HasPermission(Permissions.MustBeSameUser.ToString());
    }
}