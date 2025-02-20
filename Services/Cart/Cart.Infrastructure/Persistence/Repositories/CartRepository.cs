using Abstractions.ResultsPattern;
using Cart.Domain.Errors;
using Cart.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cart.Infrastructure.Persistence.Repositories;

public class CartRepository(CartDbContext dbContext) : ICartRepository
{
    public async Task<Result<IEnumerable<Domain.Entities.Cart>>> GetCartsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var carts = await dbContext.Carts
                .Include(c => c.Items) // Include related cart items
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<Domain.Entities.Cart>>.Success(carts);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Domain.Entities.Cart>>.Failure(CartErrors.DatabaseOperationFailed(ex.Message));
        }
    }

    public async Task<Result<Domain.Entities.Cart?>> GetCartByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var cart = await dbContext.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
            return cart is not null
                ? Result<Domain.Entities.Cart?>.Success(cart)
                : Result<Domain.Entities.Cart?>.Failure(CartErrors.CartNotFoundByUserId(userId));
        }
        catch (Exception ex)
        {
            return Result<Domain.Entities.Cart?>.Failure(new Error($"Failed to retrieve cart by User ID '{userId}'. Error: {ex.Message}"));
        }
    }

    public async Task<Result<Domain.Entities.Cart?>> GetCartByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var cart = await dbContext.Carts
                .Include(c => c.Items) // Include related cart items
                .FirstOrDefaultAsync(c => c.CartId == id, cancellationToken);

            return cart is not null
                ? Result<Domain.Entities.Cart?>.Success(cart)
                : Result<Domain.Entities.Cart?>.Failure(CartErrors.CartNotFound(id));
        }
        catch (Exception ex)
        {
            return Result<Domain.Entities.Cart?>.Failure(CartErrors.DatabaseOperationFailed(ex.Message));
        }
    }

    public async Task<Result> AddCartAsync(Domain.Entities.Cart cart, CancellationToken cancellationToken = default)
    {
        try
        {
            await dbContext.Carts.AddAsync(cart, cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result<Domain.Entities.Cart?>.Failure(CartErrors.DatabaseOperationFailed(ex.Message));
        }
    }

    public async Task<Result> UpdateCartAsync(Domain.Entities.Cart cart, CancellationToken cancellationToken = default)
    {
        try
        {
            var existingCart = await dbContext.Carts
                .Include(c => c.Items) // Include related cart items
                .FirstOrDefaultAsync(c => c.CartId == cart.CartId, cancellationToken);

            if (existingCart is null)
                return Result.Failure(CartErrors.CartNotFound(cart.CartId));

            existingCart.UserId = cart.UserId;
            existingCart.TotalPrice = cart.TotalPrice;
            existingCart.Items = cart.Items;
            existingCart.UpdatedAt = DateTime.UtcNow;

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result<Domain.Entities.Cart?>.Failure(CartErrors.DatabaseOperationFailed(ex.Message));
        }
    }

    public async Task<Result> DeleteCartAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var cart = await dbContext.Carts
                .FirstOrDefaultAsync(c => c.CartId == id, cancellationToken);

            if (cart is null)
                return Result.Failure(CartErrors.CartNotFound(id));

            dbContext.Carts.Remove(cart);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error($"Error deleting cart with ID '{id}': {ex.Message}"));
        }
    }

    public async Task<Result<Domain.Entities.Cart>> CreateCartAsync(Domain.Entities.Cart cart, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if a cart already exists for this user
            var existingCart = await dbContext.Carts
                .FirstOrDefaultAsync(c => c.UserId == cart.UserId, cancellationToken);

            if (existingCart is not null)
                return Result<Domain.Entities.Cart>.Failure(CartErrors.CartAlreadyExistsForUser(cart.UserId));

            // Set creation timestamp
            cart.CreatedAt = DateTime.UtcNow;
            cart.UpdatedAt = cart.CreatedAt;

            // Add the cart to the context
            var entry = await dbContext.Carts.AddAsync(cart, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            // Return the created cart
            return Result<Domain.Entities.Cart>.Success(entry.Entity);
        }
        catch (Exception ex)
        {
            return Result<Domain.Entities.Cart>.Failure(CartErrors.DatabaseOperationFailed(ex.Message));
        }
    }
}
