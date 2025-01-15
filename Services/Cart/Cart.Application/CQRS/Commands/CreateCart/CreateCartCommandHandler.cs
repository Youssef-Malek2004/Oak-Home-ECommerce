using Abstractions.ResultsPattern;
using Cart.Application.Services.Redis;
using Cart.Domain.Repositories;
using MediatR;

namespace Cart.Application.CQRS.Commands.CreateCart;

public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, Result<Domain.Entities.Cart>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IRedisService _redisService;

    public CreateCartCommandHandler(ICartRepository cartRepository, IRedisService redisService)
    {
        _cartRepository = cartRepository;
        _redisService = redisService;
    }

    public async Task<Result<Domain.Entities.Cart>> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        // Create a new cart entity
        var cart = new Domain.Entities.Cart
        {
            CartId = Guid.NewGuid(),
            UserId = request.UserId,
            Items = new List<Domain.Entities.CartItem>(),
            TotalPrice = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Save to database
        var result = await _cartRepository.CreateCartAsync(cart, cancellationToken);
        if (result.IsFailure)
            return Result<Domain.Entities.Cart>.Failure(result.Error);

        // Cache the cart
        await _redisService.SetCartAsync(cart.UserId, cart);

        return Result<Domain.Entities.Cart>.Success(cart);
    }
}