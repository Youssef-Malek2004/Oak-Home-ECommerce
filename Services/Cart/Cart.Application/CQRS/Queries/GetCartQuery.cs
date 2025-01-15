using Abstractions.ResultsPattern;
using Cart.Application.Services.Redis;
using Cart.Domain;
using Cart.Domain.Entities;
using MediatR;

namespace Cart.Application.CQRS.Queries;

public record GetCartQuery(Guid UserId) : IRequest<Result<Domain.Entities.Cart>>;

public class GetCartQueryHandler(IUnitOfWork unitOfWork, IRedisService redisService)
    : IRequestHandler<GetCartQuery, Result<Domain.Entities.Cart>>
{
    public async Task<Result<Domain.Entities.Cart>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cachedResult = await redisService.GetCartAsync(request.UserId);
        if (cachedResult.IsSuccess)
        {
            return cachedResult;
        }
        
        var cartResult = await unitOfWork.CartRepository.GetCartByUserIdAsync(request.UserId, cancellationToken);
        if (!cartResult.IsSuccess)
        {
            return cartResult;
        }
        
        await redisService.SetCartAsync(request.UserId, cartResult.Value);

        return cartResult;
    }
}