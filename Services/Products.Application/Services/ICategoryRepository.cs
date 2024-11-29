using Abstractions.ResultsPattern;
using Products.Domain.DTOs.CategoryDtos;
using Products.Domain.Entities;

namespace Products.Application.Services;

public interface ICategoryRepository
{
    /// <summary>
    /// Retrieves all categories.
    /// </summary>
    Task<Result<IEnumerable<Category>>> GetCategories();

    /// <summary>
    /// Retrieves a single category by its ID.
    /// </summary>
    Task<Result<Category>> GetCategoryById(string id);

    /// <summary>
    /// Creates a new category.
    /// </summary>
    Task<Result<Category>> CreateCategory(CreateCategoryDto createCategoryDto);

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    Task<Result<Category>> UpdateCategory(string id, UpdateCategoryDto updateCategoryDto);

    /// <summary>
    /// Deletes a category by its ID.
    /// </summary>
    Task<Result<bool>> DeleteCategory(string id);

    /// <summary>
    /// Checks if a category exists by its ID.
    /// </summary>
    Task<Result<bool>> CategoryExists(string id);

    /// <summary>
    /// Searches for categories by name.
    /// </summary>
    Task<Result<IEnumerable<Category>>> SearchCategories(string searchTerm);
}