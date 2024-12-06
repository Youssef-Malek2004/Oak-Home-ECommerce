using Abstractions.ResultsPattern;

namespace Orders.Domain.Errors;

public static class OrderErrors
{
    public static Error OrderNotFound(Guid orderId) =>
        new Error($"Order with ID '{orderId}' was not found.");

    public static Error OrderItemsNotFound(Guid orderId) =>
        new Error($"No items found for Order ID '{orderId}'.");

    public static Error OrderAddFailed(string message) =>
        new Error($"Failed to add order. Error: {message}");

    public static Error OrderRemoveFailed(string message) =>
        new Error($"Failed to remove order. Error: {message}");

    public static Error OrderUpdateFailed(string message) =>
        new Error($"Failed to update order. Error: {message}");

    public static Error OrderQueryFailed(string message) =>
        new Error($"Failed to query orders. Error: {message}");

    public static Error OrderItemQueryFailed(string message) =>
        new Error($"Failed to query order items. Error: {message}");
}