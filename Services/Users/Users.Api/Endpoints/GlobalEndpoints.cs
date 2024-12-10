using MediatR;
using Microsoft.AspNetCore.Mvc;
using Users.Application.CQRS.Commands;
using Users.Domain.DTOs;
using Users.Domain.Entities;

namespace Users.Api.Endpoints;

public static class GlobalEndpoints
{
    public static void MapGlobalEndpoints(this IEndpointRouteBuilder app)
    {
        var globalEndpoints = app.MapGroup("users");
        
        globalEndpoints.MapPost("/signup/user", async ([FromBody] SignUpDto signUpDto ,IMediator mediator) =>
        {
            var result = await mediator.Send(new SignUpCommand(signUpDto,Role.Registered));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        });
        
        globalEndpoints.MapPost("/signup/vendor", async ([FromBody] SignUpDto signUpDto ,IMediator mediator) =>
        {
            var result = await mediator.Send(new SignUpCommand(signUpDto,Role.Vendor));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        });
        
        // globalEndpoints.MapPost("/signup/admin", async ([FromBody] SignUpDto signUpDto ,IMediator mediator) =>
        // {
        //     var result = await mediator.Send(new SignUpCommand(signUpDto,Role.Admin));
        //     return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        // });

        globalEndpoints.MapPost("/login", async ([FromBody] LoginDto loginDto, HttpContext httpContext, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new LoginCommand(loginDto, httpContext), cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        });
        
        globalEndpoints.MapPost("/refresh-token", async (HttpContext httpContext, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new RefreshTokenCommand(httpContext), cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.Unauthorized();
        });

        
        globalEndpoints.MapPost("/logout", (HttpContext httpContext) =>
        {
            httpContext.Response.Cookies.Append("auth_token", "", new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1)
            });

            return Results.Ok();
        });
    }
}