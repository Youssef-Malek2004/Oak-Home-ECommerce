using System.Linq.Expressions;
using Abstractions.ResultsPattern;
using Inventory.Domain.Entities;

namespace Inventory.Domain.Repositories;

public interface IInventoryRepository
{
    Task<Result<IEnumerable<Inventories>>> GetInventoriesAsync(CancellationToken cancellationToken = default);
    Task<Result<Inventories?>> GetInventoryByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<Inventories>>> GetInventoriesByProductIdAsync(string productId, CancellationToken cancellationToken = default);

    Task<Result> AddInventoryAsync(Inventories inventory);

    Task<Result> RemoveInventoryAsync(Inventories inventory);

    Task<Result> UpdateInventoryAsync(Inventories inventory);

    Task<Result<IEnumerable<Inventories>>> GetInventoriesByConditionAsync(
        Expression<Func<Inventories, bool>> predicate,
        CancellationToken cancellationToken = default);
}