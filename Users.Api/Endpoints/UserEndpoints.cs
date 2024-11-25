using MediatR;
using Microsoft.AspNetCore.Mvc;
using Users.Application.CQRS.Commands;
using Users.Domain.DTOs;

namespace Users.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var userEndpoints = app.MapGroup("users").RequireAuthorization();

        userEndpoints.MapPut("{id:guid}/change-password", async (
            Guid id
            ,[FromBody] ChangePasswordDto changePasswordDto
            , IMediator mediator) =>
        {
            var result = await mediator.Send(new ChangePasswordCommand(id, changePasswordDto));
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);

        });
        
        userEndpoints.MapPut("{id:guid}/edit-profile", async (
            Guid id
            ,[FromBody] EditProfileDto editProfileDto
            , IMediator mediator) =>
        {
            var result = await mediator.Send(new EditProfileCommand(id, editProfileDto));
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);

        });
    }
}