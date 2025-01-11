using Abstractions.ResultsPattern;

namespace Cart.Domain.Errors;

public static class CartErrors
{
    public static Error CartNotFound(Guid cartId) =>
        new Error($"Cart with ID '{cartId}' was not found.");
    public static Error CartNotFoundByUserId(Guid userId) =>
        new Error($"Cart for User ID '{userId}' was not found.");
    public static Error CartItemNotFound(Guid cartId, string productId) =>
        new Error($"Cart item with Product ID '{productId}' in Cart ID '{cartId}' was not found.");
    public static Error DatabaseOperationFailed(string message) =>
        new Error($"A database operation failed: {message}");

    public static Error CartItemAddFailed(string message) =>
        new Error($"Failed to add cart item. Error: {message}");

    public static Error CartItemRemoveFailed(string message) =>
        new Error($"Failed to remove cart item. Error: {message}");

    public static Error CartItemUpdateFailed(string message) =>
        new Error($"Failed to update cart item. Error: {message}");

    public static Error CartItemsQueryFailed(string message) =>
        new Error($"Failed to query cart items. Error: {message}");
}