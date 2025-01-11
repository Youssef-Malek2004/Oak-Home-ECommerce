using Cart.Domain;
using Cart.Domain.Repositories;

namespace Cart.Infrastructure.Persistence;

public class UnitOfWork(CartDbContext dbContext,
    ICartRepository cartRepository,
    ICartItemRepository cartItemRepository) : IUnitOfWork
{
    public ICartRepository CartRepository { get; } = cartRepository;
    public ICartItemRepository CartItemRepository { get; } = cartItemRepository;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        dbContext.Dispose();
    }
}