using Microsoft.AspNetCore.Mvc;
using Products.Application.Services;
using Products.Domain.DTOs;

namespace Products.Api.Endpoints;

public static class ProductsEndpoints
{
    public static void MapProductsCrudEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("products", async (IProductsRepository productsRepository) =>
        {
            var result = await productsRepository.GetProducts();
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });

        app.MapGet("products/{id}", async (string id, IProductsRepository productsRepository) =>
        {
            var result = await productsRepository.GetProductById(id);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });

        app.MapPost("products", async ([FromBody] CreateProductDto createProductDto, IProductsRepository productsRepository) =>
        {
            var result = await productsRepository.CreateProduct(createProductDto);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        });

        app.MapPut("products/{id}", async (string id, [FromBody] UpdateProductDto updateProductDto, IProductsRepository productsRepository) =>
        {
            var result = await productsRepository.UpdateProduct(id, updateProductDto);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        });

        app.MapDelete("products/{id}", async (string id, IProductsRepository productsRepository) =>
        {
            var result = await productsRepository.DeleteProduct(id);
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        });

        app.MapGet("products/category/{categoryId}", async (string categoryId, IProductsRepository productsRepository) =>
        {
            var products = await productsRepository.GetProductsByCategory(categoryId);
            return products.IsSuccess ? Results.Ok(products.Value) : Results.NotFound(products.Error);
        });

        app.MapGet("products/search/{searchTerm}", async (string searchTerm, IProductsRepository productsRepository) =>
        {
            var products = await productsRepository.SearchProducts(searchTerm);
            return products.IsSuccess ? Results.Ok(products.Value) : Results.NotFound(products.Error);
        });
    }
}