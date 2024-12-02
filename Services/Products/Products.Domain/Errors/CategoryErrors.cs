using Abstractions.ResultsPattern;

namespace Products.Domain.Errors;

public static class CategoryErrors
{
    public static readonly Error DoesNotExist = new("Category.DoesNotExist", "The specified category does not exist.");
    public static Error CategoryNotFoundId(string id) => new("Category.CategoryNotFound", $"Category with ID '{id}' was not found.");
    public static Error CategoryAlreadyExists(string name) => new("Category.CategoryAlreadyExists", $"A category with the name '{name}' already exists.");
    public static Error CategoryAddFailed(string exceptionMessage) => new("Category.CategoryAddFailed", $"Failed to add category: {exceptionMessage}");
    public static Error CategoryUpdateFailed(string exceptionMessage) => new("Category.CategoryUpdateFailed", $"Failed to update category: {exceptionMessage}");
    public static Error CategoryDeleteFailed(string exceptionMessage) => new("Category.CategoryDeleteFailed", $"Failed to delete category: {exceptionMessage}");
    public static Error InvalidCategoryData(string reason) => new("Category.InvalidCategoryData", $"Invalid category data: {reason}");
}
