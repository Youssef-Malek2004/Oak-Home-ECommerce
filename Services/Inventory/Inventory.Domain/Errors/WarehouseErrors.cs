using Abstractions.ResultsPattern;

namespace Inventory.Domain.Errors;

public static class WarehouseErrors
{
    public static Error WarehouseNotFound(Guid warehouseId) =>
        new Error($"Warehouse with ID '{warehouseId}' was not found.");

    public static Error WarehouseQueryFailed(string message) =>
        new Error($"Failed to query warehouses. Error: {message}");

    public static Error WarehouseAddFailed(string message) =>
        new Error($"Failed to add warehouse. Error: {message}");

    public static Error WarehouseRemoveFailed(string message) =>
        new Error($"Failed to remove warehouse. Error: {message}");

    public static Error WarehouseUpdateFailed(string message) =>
        new Error($"Failed to update warehouse. Error: {message}");
    public static Error WarehousesNotFoundForProduct(string productId) =>
        new Error($"No warehouses found selling the product with ID '{productId}'.");
    public static Error FailedToAddInventory(Guid warehouseId, Guid inventoryId) =>
        new Error("WarehouseErrors.FailedToAddInventory",
            $"Failed to add Inventory: {inventoryId} to Warehouse: {warehouseId}");
    public static Error FailedToRemoveInventory(Guid warehouseId, Guid inventoryId) =>
        new Error("WarehouseErrors.FailedToRemoveInventory",
            $"Failed to remove Inventory: {inventoryId} from Warehouse: {warehouseId}");
    
    public static Error InventoryNotFoundInWarehouse(Guid warehouseId, Guid inventoryId) =>
        new Error("WarehouseErrors.InventoryNotFoundInWarehouse",
            $"Inventory: {inventoryId} not found in Warehouse: {warehouseId}");
}