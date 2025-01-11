using Cart.Domain.Repositories;

namespace Cart.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        ICartRepository CartRepository { get; }
        ICartItemRepository CartItemRepository { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}