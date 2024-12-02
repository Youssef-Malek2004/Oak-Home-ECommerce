using Abstractions.ResultsPattern;
using MongoDB.Entities;
using Products.Application.Services;
using Products.Domain.DTOs.CategoryDtos;
using Products.Domain.Entities;
using Products.Domain.Errors;

namespace Products.Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    public CategoryRepository()
    {
    }
    public async Task<Result<IEnumerable<Category>>> GetCategories()
    {
        var categories = await DB.Find<Category>().ExecuteAsync();
        return Result<IEnumerable<Category>>.Success(categories);
    }

    public async Task<Result<Category>> GetCategoryById(string id)
    {
        var category = await DB.Find<Category>().Match(c => c.ID == id).ExecuteFirstAsync();

        return category != null
            ? Result<Category>.Success(category)
            : Result<Category>.Failure(CategoryErrors.CategoryNotFoundId(id));
    }

    public async Task<Result<Category>> CreateCategory(CreateCategoryDto createCategoryDto)
    {
        var category = new Category
        {
            Name = createCategoryDto.Name
        };

        await DB.SaveAsync(category);

        return Result<Category>.Success(category);
    }

    public async Task<Result<Category>> UpdateCategory(string id, UpdateCategoryDto updateCategoryDto)
    {
        var category = await DB.Find<Category>().Match(c => c.ID == id).ExecuteFirstAsync();

        if (category == null)
        {
            return Result<Category>.Failure(CategoryErrors.CategoryUpdateFailed(CategoryErrors.CategoryNotFoundId(id).Code!));
        }

        if (!string.IsNullOrEmpty(updateCategoryDto.Name))
        {
            category.Name = updateCategoryDto.Name;
        }

        await DB.SaveAsync(category);

        return Result<Category>.Success(category);
    }

    public async Task<Result<bool>> DeleteCategory(string id)
    {
        var result = await DB.DeleteAsync<Category>(id);

        return result.DeletedCount > 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(CategoryErrors.CategoryDeleteFailed(CategoryErrors.CategoryNotFoundId(id).Code!));
    }

    public async Task<Result<bool>> CategoryExists(string id)
    {
        var exists = await DB.Find<Category>().Match(c => c.ID == id).ExecuteAnyAsync();

        return Result<bool>.Success(exists);
    }

    public async Task<Result<IEnumerable<Category>>> SearchCategories(string searchTerm)
    {
        var categories = await DB.Find<Category>()
            .Match(c => c.Name.ToLower().Contains(searchTerm.ToLower()))
            .ExecuteAsync();

        return Result<IEnumerable<Category>>.Success(categories);
    }
}