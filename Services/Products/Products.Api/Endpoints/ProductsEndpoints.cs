using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.Application.CQRS.CommandsAndQueries.Products;
using Products.Domain.DTOs.ProductDtos;
using Products.Domain.Entities;
using Products.Infrastructure.Authentication;
using Shared.Contracts.Authorization;

namespace Products.Api.Endpoints;

public static class ProductsEndpoints
{
    public static void MapProductsCrudEndpoints(this IEndpointRouteBuilder app)
    {
        var productsEndpoints = app.MapGroup("products").RequireAuthorization();
        
        productsEndpoints.MapGet("", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllProductsQuery());
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });

        productsEndpoints.MapGet("{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetProductByIdQuery(id));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });

        productsEndpoints.MapPost("", async ([FromBody] CreateProductRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateProductCommand(request.AddProductInventoryFields, request.CreateProductDto, request.DynamicFields));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        }).HasRole(Roles.Vendor.Name);

        productsEndpoints.MapPut("{id}", async (string id, [FromBody] UpdateProductRequest updateProductRequest, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateProductCommand(id, updateProductRequest.UpdateProductDto, updateProductRequest.DynamicFields));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        });

        productsEndpoints.MapDelete("{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteProductCommand(id));
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        });
        
        productsEndpoints.MapDelete("{id}/toggle-soft-deletion", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new ToggleSoftDeletionProductCommand(id));
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        });

        productsEndpoints.MapGet("category/{categoryId}", async (string categoryId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetProductsByCategoryQuery(categoryId));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });

        productsEndpoints.MapGet("search/{searchTerm}", async (string searchTerm, IMediator mediator) =>
        {
            var result = await mediator.Send(new SearchProductsQuery(searchTerm));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });
        
        productsEndpoints.MapPut("{id}/toggle-featured", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new ToggleFeaturedProductCommand(id));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        });
    }
}