using MediatR;
using Microsoft.AspNetCore.Mvc;
using Users.Application.CQRS.Commands;
using Users.Domain.DTOs;
using Users.Infrastructure.Authentication;

namespace Users.Api.Endpoints;

public static class AdminEndpoints
{
    public static void MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        var adminEndpoints = app.MapGroup("admins").RequireAuthorization();

        adminEndpoints.MapPost("", async (IMediator mediator, [FromBody] AddAdminDto addAdminDto, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new AddAdminCommand(addAdminDto), cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        }).HasPermission(Permissions.PerformCrud); //Admin Permissions
    }
}