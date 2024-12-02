using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.Application.CQRS.CommandsAndQueries.Categories;
using Products.Domain.DTOs.CategoryDtos;

namespace Products.Api.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryCrudEndpoints(this IEndpointRouteBuilder app)
    {
        var categoriesEndpoints = app.MapGroup("categories").RequireAuthorization();
        
        categoriesEndpoints.MapGet("", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllCategoriesQuery());
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error.Description);
        });

        categoriesEndpoints.MapGet("{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetCategoryByIdQuery(id));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error.Description);
        });

        categoriesEndpoints.MapPost("", async ([FromBody] CreateCategoryDto createCategoryDto, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateCategoryCommand(createCategoryDto));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error.Description);
        });

        categoriesEndpoints.MapPut("{id}", async (string id, [FromBody] UpdateCategoryDto updateCategoryDto, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateCategoryCommand(id, updateCategoryDto));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error.Description);
        });

        categoriesEndpoints.MapDelete("{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteCategoryCommand(id));
            return result.IsSuccess ? Results.Ok() : Results.NotFound(result.Error.Description);
        });

        categoriesEndpoints.MapGet("exists/{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new CategoryExistsQuery(id));
            return Results.Ok(result.Value);
        });

        categoriesEndpoints.MapGet("search/{searchTerm}", async (string searchTerm, IMediator mediator) =>
        {
            var result = await mediator.Send(new SearchCategoriesQuery(searchTerm));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error.Description);
        });
    }
}