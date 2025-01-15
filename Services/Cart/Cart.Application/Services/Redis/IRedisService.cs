using Abstractions.ResultsPattern;
using Cart.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Cart.Application.Services.Redis;

public interface IRedisService
{
    Task<Result<Domain.Entities.Cart>> GetCartAsync(Guid userId);
    Task<Result> SetCartAsync(Guid userId, Domain.Entities.Cart cart);
    Task<Result> UpdateCartAsync(Guid userId, Domain.Entities.Cart cart);
    Task<Result> DeleteCartAsync(Guid userId);
    Task<Result> InvalidateCacheAsync(Guid userId);
}