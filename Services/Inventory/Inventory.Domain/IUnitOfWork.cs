using Inventory.Domain.Repositories;

namespace Inventory.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IInventoryRepository InventoryRepository { get; }
        IWarehouseRepository WarehouseRepository { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}