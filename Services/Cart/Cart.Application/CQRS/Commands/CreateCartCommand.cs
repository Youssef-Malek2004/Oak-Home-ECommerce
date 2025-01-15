using Abstractions.ResultsPattern;
using Cart.Application.Services.Redis;
using Cart.Domain;
using Cart.Domain.Entities;
using Cart.Domain.Errors;
using MediatR;

namespace Cart.Application.CQRS.Commands;

public record CreateCartCommand(Guid UserId) : IRequest<Result<Domain.Entities.Cart>>;

public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, Result<Domain.Entities.Cart>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisService _redisService;

    public CreateCartCommandHandler(IUnitOfWork unitOfWork, IRedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _redisService = redisService;
    }

    public async Task<Result<Domain.Entities.Cart>> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        // Check if cart already exists
        var existingCart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(request.UserId, cancellationToken);
        if (existingCart.IsSuccess)
        {
            return existingCart;
        }

        // Create new cart
        var cart = new Domain.Entities.Cart
        {
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Items = new List<CartItem>()
        };

        // Save to database
        var result = await _unitOfWork.CartRepository.CreateCartAsync(cart, cancellationToken);
        if (!result.IsSuccess)
        {
            return Result<Domain.Entities.Cart>.Failure(CartErrors.DatabaseOperationFailed("Failed to create cart"));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cache the new cart
        await _redisService.SetCartAsync(request.UserId, cart);

        return Result<Domain.Entities.Cart>.Success(cart);
    }
}