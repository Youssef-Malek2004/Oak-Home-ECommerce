using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.Application.CQRS.CommandsAndQueries.Products;
using Products.Domain.DTOs.ProductDtos;

namespace Products.Api.Endpoints;

public static class ProductsEndpoints
{
    public static void MapProductsCrudEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("products", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllProductsQuery());
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });

        app.MapGet("products/{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetProductByIdQuery(id));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });

        app.MapPost("products", async ([FromBody] CreateProductRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateProductCommand(request.CreateProductDto, request.DynamicFields));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        });

        app.MapPut("products/{id}", async (string id, [FromBody] UpdateProductRequest updateProductRequest, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateProductCommand(id, updateProductRequest.UpdateProductDto, updateProductRequest.DynamicFields));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        });

        app.MapDelete("products/{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteProductCommand(id));
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        });
        
        app.MapDelete("products/{id}/toggle-soft-deletion", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new ToggleSoftDeletionProductCommand(id));
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        });

        app.MapGet("products/category/{categoryId}", async (string categoryId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetProductsByCategoryQuery(categoryId));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });

        app.MapGet("products/search/{searchTerm}", async (string searchTerm, IMediator mediator) =>
        {
            var result = await mediator.Send(new SearchProductsQuery(searchTerm));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });
        
        app.MapPut("products/{id}/toggle-featured", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new ToggleFeaturedProductCommand(id));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        });
    }
}