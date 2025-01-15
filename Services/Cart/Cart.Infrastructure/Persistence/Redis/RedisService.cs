using System.Text.Json;
using Abstractions.ResultsPattern;
using Cart.Application.Services.Redis;
using Cart.Domain.Errors;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Cart.Infrastructure.Persistence.Redis;

public class RedisService(
    IDistributedCache distributedCache,
    IMemoryCache memoryCache,
    IOptions<DistributedCacheEntryOptions> cacheOptions)
    : IRedisService
{
    private readonly DistributedCacheEntryOptions _cacheOptions = cacheOptions.Value;
    private readonly MemoryCacheEntryOptions _memoryCacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
        SlidingExpiration = TimeSpan.FromMinutes(2)
    };

    private static string GetCartKey(Guid userId) => $"cart:user:{userId}";

    public async Task<Result<Domain.Entities.Cart>> GetCartAsync(Guid userId)
    {
        try
        {
            var cacheKey = GetCartKey(userId);
            
            if (memoryCache.TryGetValue(cacheKey, out Domain.Entities.Cart? memoryCart))
            {
                return Result<Domain.Entities.Cart>.Success(memoryCart!);
            }
            
            var cachedCart = await distributedCache.GetAsync(cacheKey);
            if (cachedCart is null)
            {
                return Result<Domain.Entities.Cart>.Failure(CartErrors.CartNotFoundByUserId(userId));
            }

            var cart = JsonSerializer.Deserialize<Domain.Entities.Cart>(cachedCart);
            if (cart is null)
            {
                return Result<Domain.Entities.Cart>.Failure(CartErrors.CartNotFoundByUserId(userId));
            }
            
            memoryCache.Set(cacheKey, cart, _memoryCacheOptions);

            return Result<Domain.Entities.Cart>.Success(cart);
        }
        catch (Exception)
        {
            return Result<Domain.Entities.Cart>.Failure(CartErrors.FailedToGetCart(userId));
        }
    }

    public async Task<Result> SetCartAsync(Guid userId, Domain.Entities.Cart cart)
    {
        try
        {
            var cacheKey = GetCartKey(userId);
            var serializedCart = JsonSerializer.SerializeToUtf8Bytes(cart);

            // Set in distributed cache
            await distributedCache.SetAsync(cacheKey, serializedCart, _cacheOptions);

            // Set in memory cache
            memoryCache.Set(cacheKey, cart, _memoryCacheOptions);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(CartErrors.FailedToSetCart(userId));
        }
    }

    public async Task<Result> UpdateCartAsync(Guid userId, Domain.Entities.Cart cart)
    {
        try
        {
            var result = await SetCartAsync(userId, cart);
            if (!result.IsSuccess)
            {
                return Result.Failure(CartErrors.FailedToUpdateCart(userId));
            }
            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(CartErrors.FailedToUpdateCart(userId));
        }
    }

    public async Task<Result> DeleteCartAsync(Guid userId)
    {
        try
        {
            var cacheKey = GetCartKey(userId);

            // Remove from distributed cache
            await distributedCache.RemoveAsync(cacheKey);

            // Remove from memory cache
            memoryCache.Remove(cacheKey);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(CartErrors.FailedToDeleteCart(userId));
        }
    }

    public async Task<Result> InvalidateCacheAsync(Guid userId)
    {
        return await DeleteCartAsync(userId);
    }
}