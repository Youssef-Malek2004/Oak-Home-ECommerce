using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Authorization;
using Users.Application.CQRS.Commands;
using Users.Domain;
using Users.Domain.Entities;
using Users.Infrastructure.Authentication;
using Users.Infrastructure.Persistence;

namespace Users.Api.Endpoints;

public static class CrudEndpoints
{
    public static void MapCrudEndpoints(this IEndpointRouteBuilder app)
    {
        var crudEndpoints = app.MapGroup("users").RequireAuthorization();
        
        crudEndpoints.MapDelete("/{id:Guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken ) =>
        {
            var result = await mediator.Send(new SoftDeleteCommand(id), cancellationToken);
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        }).HasPermission(Permissions.PerformCrud);

        crudEndpoints.MapGet("/", async (UsersDbContext usersDbContext) =>
        {
            var users = await usersDbContext.Set<User>().ToListAsync();
            return Results.Ok(users);
        });

        crudEndpoints.MapGet("/{id:guid}", async (Guid id, IUnitOfWork unitOfWork) =>
        {
            var result = await unitOfWork.UserRepository.GetUserByIdAsync(id);

            if (result.IsFailure)
            {
                return Results.NotFound(new
                {
                    Code = result.Error.Code,
                    Message = result.Error.Description
                });
            }

            return Results.Ok(result.Value);
        });

        crudEndpoints.MapGet("/{email}", async (string email, IUnitOfWork unitOfWork) =>
        {
            var result = await unitOfWork.UserRepository.GetUserByEmailAsync(email);

            if (result.IsFailure)
            {
                return Results.NotFound(new
                {
                    Code = result.Error.Code,
                    Message = result.Error.Description
                });
            }

            return Results.Ok(result.Value);
        });
        
    }
}