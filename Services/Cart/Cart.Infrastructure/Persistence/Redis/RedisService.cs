using System.Text.Json;
using Abstractions.ResultsPattern;
using Cart.Application.Services.Redis;
using Cart.Domain.Entities;
using Cart.Domain.Errors;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Cart.Infrastructure.Persistence.Redis;

public class RedisService : IRedisService
{
    private readonly IDistributedCache _distributedCache;
    private readonly IMemoryCache _memoryCache;
    private readonly DistributedCacheEntryOptions _cacheOptions;
    private readonly MemoryCacheEntryOptions _memoryCacheOptions;

    public RedisService(
        IDistributedCache distributedCache,
        IMemoryCache memoryCache,
        IOptions<DistributedCacheEntryOptions> cacheOptions)
    {
        _distributedCache = distributedCache;
        _memoryCache = memoryCache;
        _cacheOptions = cacheOptions.Value;
        _memoryCacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            SlidingExpiration = TimeSpan.FromMinutes(2)
        };
    }

    private static string GetCartKey(Guid userId) => $"cart:user:{userId}";

    public async Task<Result<Domain.Entities.Cart>> GetCartAsync(Guid userId)
    {
        try
        {
            var cacheKey = GetCartKey(userId);

            // Try memory cache first
            if (_memoryCache.TryGetValue(cacheKey, out Domain.Entities.Cart? memoryCart))
            {
                return Result<Domain.Entities.Cart>.Success(memoryCart!);
            }

            // Try distributed cache
            var cachedCart = await _distributedCache.GetAsync(cacheKey);
            if (cachedCart is null)
            {
                return Result<Domain.Entities.Cart>.Failure(CartErrors.CartNotFoundByUserId(userId));
            }

            var cart = JsonSerializer.Deserialize<Domain.Entities.Cart>(cachedCart);
            if (cart is null)
            {
                return Result<Domain.Entities.Cart>.Failure(CartErrors.CartNotFoundByUserId(userId));
            }

            // Cache in memory for subsequent requests
            _memoryCache.Set(cacheKey, cart, _memoryCacheOptions);

            return Result<Domain.Entities.Cart>.Success(cart);
        }
        catch (Exception ex)
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
            await _distributedCache.SetAsync(cacheKey, serializedCart, _cacheOptions);

            // Set in memory cache
            _memoryCache.Set(cacheKey, cart, _memoryCacheOptions);

            return Result.Success();
        }
        catch (Exception ex)
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
        catch (Exception ex)
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
            await _distributedCache.RemoveAsync(cacheKey);

            // Remove from memory cache
            _memoryCache.Remove(cacheKey);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(CartErrors.FailedToDeleteCart(userId));
        }
    }

    public async Task<Result> InvalidateCacheAsync(Guid userId)
    {
        return await DeleteCartAsync(userId);
    }
}