using System.Linq.Expressions;
using Abstractions.ResultsPattern;
using Inventory.Domain.Entities;
using Inventory.Domain.Errors;
using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class WarehouseRepository(InventoryDbContext context) : IWarehouseRepository
{
    public async Task<Result<IEnumerable<Warehouse>>> GetWarehousesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var warehouses = await context.Warehouses
                .Include(w => w.Address)
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<Warehouse>>.Success(warehouses);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Warehouse>>.Failure(WarehouseErrors.WarehouseQueryFailed(ex.Message));
        }
    }

    public async Task<Result<Warehouse?>> GetWarehouseByIdAsync(Guid warehouseId, CancellationToken cancellationToken = default)
    {
        try
        {
            var warehouse = await context.Warehouses
                .Include(w => w.Address)
                .FirstOrDefaultAsync(w => w.WarehouseId == warehouseId, cancellationToken);

            if (warehouse == null)
            {
                return Result<Warehouse?>.Failure(WarehouseErrors.WarehouseNotFound(warehouseId));
            }

            return Result<Warehouse?>.Success(warehouse);
        }
        catch (Exception ex)
        {
            return Result<Warehouse?>.Failure(WarehouseErrors.WarehouseQueryFailed(ex.Message));
        }
    }

    public async Task<Result> AddWarehouseAsync(Warehouse warehouse)
    {
        try
        {
            await context.Warehouses.AddAsync(warehouse);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(WarehouseErrors.WarehouseAddFailed(ex.Message));
        }
    }

    public async Task<Result> RemoveWarehouseAsync(Warehouse warehouse)
    {
        try
        {
            context.Warehouses.Remove(warehouse);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(WarehouseErrors.WarehouseRemoveFailed(ex.Message));
        }
    }

    public async Task<Result> UpdateWarehouseAsync(Warehouse warehouse)
    {
        try
        {
            context.Warehouses.Update(warehouse);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(WarehouseErrors.WarehouseUpdateFailed(ex.Message));
        }
    }

    public async Task<Result> AddInventoryToWarehouse(Warehouse warehouse, Inventories inventory)
    {
        try
        {
            warehouse.Inventories.Add(inventory);
            context.Warehouses.Update(warehouse);
            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(WarehouseErrors.FailedToAddInventory(warehouse.WarehouseId, inventory.Id));
        }
    }

    public async Task<Result> RemoveInventoryFromWarehouse(Warehouse warehouse, Inventories inventory)
    {
        try
        {
            if (!warehouse.Inventories.Contains(inventory))
                return Result.Failure(WarehouseErrors.InventoryNotFoundInWarehouse(warehouse.WarehouseId,inventory.Id));
            
            warehouse.Inventories.Remove(inventory);
            context.Warehouses.Update(warehouse);
            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(WarehouseErrors.FailedToRemoveInventory(warehouse.WarehouseId, inventory.Id));
        }
    }

    public async Task<Result<IEnumerable<Warehouse>>> GetWarehousesSellingProductAsync(string productId, CancellationToken cancellationToken = default)
    {
        try
        {
            var warehouses = await context.Warehouses
                .Include(w => w.Inventories)
                .Where(w => w.Inventories.Any(i => i.ProductId == productId && i.Quantity >= 0)) 
                .ToListAsync(cancellationToken);

            if (!warehouses.Any())
            {
                return Result<IEnumerable<Warehouse>>.Failure(WarehouseErrors.WarehousesNotFoundForProduct(productId));
            }

            return Result<IEnumerable<Warehouse>>.Success(warehouses);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Warehouse>>.Failure(WarehouseErrors.WarehouseQueryFailed(ex.Message));
        }
    }


    public async Task<Result<IEnumerable<Warehouse>>> GetWarehousesByConditionAsync(
        Expression<Func<Warehouse, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var warehouses = await context.Warehouses
                .Where(predicate)
                .Include(w => w.Address)
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<Warehouse>>.Success(warehouses);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Warehouse>>.Failure(WarehouseErrors.WarehouseQueryFailed(ex.Message));
        }
    }
}