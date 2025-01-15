using Cart.Application.CQRS.Queries;
using Cart.Application.CQRS.Commands.CreateCart;
using MediatR;

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
        });

        app.MapPost("cart/{userId:guid}", async (Guid userId, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateCartCommand(userId);
            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.Created($"/api/cart/{userId}", result.Value);
        })
        .WithName("CreateCart")
        .WithOpenApi()
        .RequireAuthorization();
    }
}