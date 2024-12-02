using Inventory.Domain;
using Inventory.Domain.Repositories;

namespace Inventory.Infrastructure.Persistence
{
    public class UnitOfWork(InventoryDbContext dbContext, IInventoryRepository inventoryRepository) : IUnitOfWork
    {
        public IInventoryRepository InventoryRepository { get; } = inventoryRepository;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}