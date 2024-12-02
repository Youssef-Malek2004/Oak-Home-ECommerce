using System.Linq.Expressions;
using Abstractions.ResultsPattern;
using Inventory.Domain.Entities;
using Inventory.Domain.Errors;
using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class InventoryRepository(InventoryDbContext dbContext) : IInventoryRepository
{
    public async Task<Result<IEnumerable<Inventories>>> GetInventoriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var inventories = await dbContext.Inventory
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<Inventories>>.Success(inventories);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Inventories>>.Failure(InventoryErrors.InventoryQueryFailed(ex.Message));
        }
    }

    public async Task<Result<Inventories?>> GetInventoryByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var inventory = await dbContext.Inventory
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        if (inventory == null)
        {
            return Result<Inventories?>.Failure(InventoryErrors.InventoryNotFoundId(id));
        }

        return Result<Inventories?>.Success(inventory);
    }

    public async Task<Result<Inventories>> GetInventoriesByProductIdAsync(string productId, CancellationToken cancellationToken = default)
    {
        var inventories = await dbContext.Inventory
            .FirstOrDefaultAsync(i => i.ProductId == productId, cancellationToken: cancellationToken);

        if (inventories is null)
        {
            return Result<Inventories>.Failure(InventoryErrors.InventoryNotFoundProductId(productId));
        }

        return Result<Inventories>.Success(inventories);
    }

    public async Task<Result> AddInventoryAsync(Inventories inventory)
    {
        try
        {
            await dbContext.Inventory.AddAsync(inventory);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            return InventoryErrors.InventoryAddFailed(ex.Message);
        }
    }
    
    public async Task<Result> SetSoftDelete(Inventories inventory, bool isDeleted, CancellationToken cancellationToken = default)
    {
        try
        {
            inventory.IsDeleted = isDeleted;
            dbContext.Inventory.Update(inventory);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(InventoryErrors.InventoryUpdateFailed(ex.Message));
        }
    }

    public async Task<Result> RemoveInventoryAsync(Inventories inventory)
    {
        try
        {
            dbContext.Inventory.Remove(inventory);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            return InventoryErrors.InventoryRemoveFailed(ex.Message);
        }
    }

    public async Task<Result> UpdateInventoryAsync(Inventories inventory)
    {
        try
        {
            dbContext.Inventory.Update(inventory);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            return InventoryErrors.InventoryUpdateFailed(ex.Message);
        }
    }

    public async Task<Result<IEnumerable<Inventories>>> GetInventoriesByConditionAsync(
        Expression<Func<Inventories, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var inventories = await dbContext.Inventory
                .Where(predicate)
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<Inventories>>.Success(inventories);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Inventories>>.Failure(InventoryErrors.InventoryQueryFailed(ex.Message));
        }
    }
}
