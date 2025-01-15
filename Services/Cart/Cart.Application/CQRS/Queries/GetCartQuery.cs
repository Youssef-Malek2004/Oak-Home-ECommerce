using Abstractions.ResultsPattern;
using Cart.Application.Services.Redis;
using Cart.Domain;
using Cart.Domain.Entities;
using MediatR;

namespace Cart.Application.CQRS.Queries;

public record GetCartQuery(Guid UserId) : IRequest<Result<Domain.Entities.Cart>>;

public class GetCartQueryHandler : IRequestHandler<GetCartQuery, Result<Domain.Entities.Cart>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisService _redisService;

    public GetCartQueryHandler(IUnitOfWork unitOfWork, IRedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _redisService = redisService;
    }

    public async Task<Result<Domain.Entities.Cart>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        // Try to get from cache first
        var cachedResult = await _redisService.GetCartAsync(request.UserId);
        if (cachedResult.IsSuccess)
        {
            return cachedResult;
        }

        // If not in cache, get from database
        var cartResult = await _unitOfWork.CartRepository.GetCartByUserIdAsync(request.UserId, cancellationToken);
        if (!cartResult.IsSuccess)
        {
            return cartResult;
        }

        // Cache the result
        await _redisService.SetCartAsync(request.UserId, cartResult.Value);

        return cartResult;
    }
}