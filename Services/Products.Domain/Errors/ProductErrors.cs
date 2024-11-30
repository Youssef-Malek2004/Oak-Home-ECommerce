using Abstractions.ResultsPattern;

namespace Products.Domain.Errors;

public static class ProductErrors
{
    public static readonly Error DoesntExist = new("Products.DoesntExist", "Product does not exist.");
    public static Error ProductNotFoundId(string id) => new("Products.ProductNotFound", $"Product with ID {id} was not found.");
    public static Error ProductNotFoundCategory(string categoryId) => new("Products.ProductNotFoundCategory", $"No products were found for category ID {categoryId}.");
    public static Error ProductAddFailed(string exceptionMessage) => new("Products.ProductAddFailed", $"Failed to add product: {exceptionMessage}");
    public static Error ProductRemoveFailed(string exceptionMessage) => new("Products.ProductRemoveFailed", $"Failed to remove product: {exceptionMessage}");
    public static Error ProductEditFailed(string exceptionMessage) => new("Products.ProductEditFailed", $"Failed to edit product: {exceptionMessage}");
    public static Error ProductAlreadyExists(string sku) => new("Products.ProductAlreadyExists", $"A product with SKU '{sku}' already exists.");
    public static Error InvalidProductData(string reason) => new("Products.InvalidProductData", $"Invalid product data: {reason}");
    public static Error ProductUpdateFailedDeleted(string id) => new("Products.ProductUpdateFailedDeleted", $"Product {id} is marked as deleted and cannot be updated");
    public static Error ProductToggleFeatureFailed(string message) => new("Product.ToggleFeatureFailed", message);
    
}