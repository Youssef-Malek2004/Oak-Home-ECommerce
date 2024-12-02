using Inventory.Domain.Repositories;

namespace Inventory.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IInventoryRepository InventoryRepository { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}