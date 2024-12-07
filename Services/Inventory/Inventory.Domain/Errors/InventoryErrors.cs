using Abstractions.ResultsPattern;

namespace Inventory.Domain.Errors;

public static class InventoryErrors
{
    public static Error InventoryNotFoundId(Guid id) =>
        new Error($"Inventory with ID '{id}' was not found.");

    public static Error InventoryNotFoundProductId(string productId) =>
        new Error($"No inventory records found for Product ID '{productId}'.");

    public static Error InventoryAddFailed(string message) =>
        new Error($"Failed to add inventory. Error: {message}");

    public static Error InventoryRemoveFailed(string message) =>
        new Error($"Failed to remove inventory. Error: {message}");

    public static Error InventoryUpdateFailed(string message) =>
        new Error($"Failed to update inventory. Error: {message}");

    public static Error InventoryQueryFailed(string message) =>
        new Error($"Failed to query inventories. Error: {message}");
    public static Error InventoryNotEnough(string message) =>
        new Error($"Error: {message}");
    public static Error FailedToSupply(Guid id) =>
        new Error("InventoryErrors.FailedToSupply",$"Failed to Supply inventory with id: {id}");
}
