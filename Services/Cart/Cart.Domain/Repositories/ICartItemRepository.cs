using Abstractions.ResultsPattern;
using Cart.Domain.Entities;

namespace Cart.Domain.Repositories;

public interface ICartItemRepository
{
    Task<Result<IEnumerable<CartItem>>> GetCartItemsAsync(Guid cartId, CancellationToken cancellationToken = default);
    Task<Result<CartItem?>> GetCartItemByIdAsync(Guid cartId, string productId, CancellationToken cancellationToken = default);
    Task<Result> AddCartItemAsync(Guid cartId, CartItem cartItem, CancellationToken cancellationToken = default);
    Task<Result> UpdateCartItemAsync(Guid cartId,CartItem cartItem, CancellationToken cancellationToken = default);
    Task<Result> DeleteCartItemAsync(Guid cartId, string productId, CancellationToken cancellationToken = default);
}