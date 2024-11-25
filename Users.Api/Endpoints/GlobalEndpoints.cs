using MediatR;
using Microsoft.AspNetCore.Mvc;
using Users.Application.CQRS.Commands;
using Users.Domain.DTOs;

namespace Users.Api.Endpoints;

public static class GlobalEndpoints
{
    public static void MapGlobalEndpoints(this IEndpointRouteBuilder app)
    {
        var globalEndpoints = app.MapGroup("");
        
        globalEndpoints.MapPost("/signup", async ([FromBody] SignUpDto signUpDto ,IMediator mediator) =>
        {
            var result = await mediator.Send(new SignUpCommand(signUpDto));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        });

        globalEndpoints.MapPost("/login", async ([FromBody] LoginDto loginDto, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new LoginCommand(loginDto), cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        });
    }
}