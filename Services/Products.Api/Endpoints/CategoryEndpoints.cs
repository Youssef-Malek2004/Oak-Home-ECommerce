using Microsoft.AspNetCore.Mvc;
using Products.Application.Services;
using Products.Domain.DTOs.CategoryDtos;

namespace Products.Api.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryCrudEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("categories", async (ICategoryRepository categoryRepository) =>
        {
            var result = await categoryRepository.GetCategories();
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error.Description);
        });

        app.MapGet("categories/{id}", async (string id, ICategoryRepository categoryRepository) =>
        {
            var result = await categoryRepository.GetCategoryById(id);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error.Description);
        });

        app.MapPost("categories", async ([FromBody] CreateCategoryDto createCategoryDto, ICategoryRepository categoryRepository) =>
        {
            var result = await categoryRepository.CreateCategory(createCategoryDto);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error.Description);
        });

        app.MapPut("categories/{id}", async (string id, [FromBody] UpdateCategoryDto updateCategoryDto, ICategoryRepository categoryRepository) =>
        {
            var result = await categoryRepository.UpdateCategory(id, updateCategoryDto);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error.Description);
        });

        app.MapDelete("categories/{id}", async (string id, ICategoryRepository categoryRepository) =>
        {
            var result = await categoryRepository.DeleteCategory(id);
            return result.IsSuccess ? Results.Ok() : Results.NotFound(result.Error.Description);
        });

        app.MapGet("categories/exists/{id}", async (string id, ICategoryRepository categoryRepository) =>
        {
            var result = await categoryRepository.CategoryExists(id);
            return Results.Ok(result.Value); 
        });

        app.MapGet("categories/search/{searchTerm}", async (string searchTerm, ICategoryRepository categoryRepository) =>
        {
            var result = await categoryRepository.SearchCategories(searchTerm);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error.Description);
        });
    }
}