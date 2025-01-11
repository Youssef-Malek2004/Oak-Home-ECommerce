using Abstractions.ResultsPattern;
using Cart.Domain.Entities;
using Cart.Domain.Errors;
using Cart.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cart.Infrastructure.Persistence.Repositories;

public class CartItemRepository(CartDbContext dbContext) : ICartItemRepository
{
    public async Task<Result<IEnumerable<CartItem>>> GetCartItemsAsync(Guid cartId, CancellationToken cancellationToken = default)
    {
        try
        {
            var cart = await dbContext.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.CartId == cartId, cancellationToken);

            if (cart is null)
                return Result<IEnumerable<CartItem>>.Failure(CartErrors.CartNotFound(cartId));

            return Result<IEnumerable<CartItem>>.Success(cart.Items);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<CartItem>>.Failure(CartErrors.CartItemsQueryFailed($"Failed to retrieve items for Cart ID '{cartId}': {ex.Message}"));
        }
    }

    public async Task<Result<CartItem?>> GetCartItemByIdAsync(Guid cartId, string productId, CancellationToken cancellationToken = default)
    {
        try
        {
            var cart = await dbContext.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.CartId == cartId, cancellationToken);

            if (cart is null)
                return Result<CartItem?>.Failure(CartErrors.CartNotFound(cartId));

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            return item is not null
                ? Result<CartItem?>.Success(item)
                : Result<CartItem?>.Failure(CartErrors.CartItemNotFound(cartId, productId));
        }
        catch (Exception ex)
        {
            return Result<CartItem?>.Failure(CartErrors.CartItemsQueryFailed($"Failed to retrieve item '{productId}' in Cart ID '{cartId}': {ex.Message}"));
        }
    }

    public async Task<Result> AddCartItemAsync(Guid cartId, CartItem cartItem, CancellationToken cancellationToken = default)
    {
        try
        {
            var cart = await dbContext.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.CartId == cartId, cancellationToken);

            if (cart is null)
                return Result.Failure(CartErrors.CartNotFound(cartId));

            cart.Items.Add(cartItem);
            cart.UpdatedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(CartErrors.CartItemAddFailed($"Failed to add item '{cartItem.ProductId}' to Cart ID '{cartId}': {ex.Message}"));
        }
    }

    public async Task<Result> UpdateCartItemAsync(Guid cartId, CartItem cartItem, CancellationToken cancellationToken = default)
    {
        try
        {
            var cart = await dbContext.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.CartId == cartId, cancellationToken);

            if (cart is null)
                return Result.Failure(CartErrors.CartNotFound(cartId));

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == cartItem.ProductId);
            if (existingItem is null)
                return Result.Failure(CartErrors.CartItemNotFound(cartId, cartItem.ProductId));

            cart.Items.Remove(existingItem);
            cart.Items.Add(cartItem);
            cart.UpdatedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(CartErrors.CartItemUpdateFailed($"Failed to update item '{cartItem.ProductId}' in Cart ID '{cartId}': {ex.Message}"));
        }
    }

    public async Task<Result> DeleteCartItemAsync(Guid cartId, string productId, CancellationToken cancellationToken = default)
    {
        try
        {
            var cart = await dbContext.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.CartId == cartId, cancellationToken);

            if (cart is null)
                return Result.Failure(CartErrors.CartNotFound(cartId));

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item is null)
                return Result.Failure(CartErrors.CartItemNotFound(cartId, productId));

            cart.Items.Remove(item);
            cart.UpdatedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(CartErrors.CartItemRemoveFailed($"Failed to remove item '{productId}' from Cart ID '{cartId}': {ex.Message}"));
        }
    }
}
