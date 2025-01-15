using Abstractions.ResultsPattern;

namespace Cart.Domain.Repositories;

public interface ICartRepository
{
    Task<Result<IEnumerable<Domain.Entities.Cart>>> GetCartsAsync(CancellationToken cancellationToken = default);
    Task<Result<Domain.Entities.Cart?>> GetCartByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<Domain.Entities.Cart?>> GetCartByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result> AddCartAsync(Domain.Entities.Cart cart, CancellationToken cancellationToken = default);
    Task<Result<Domain.Entities.Cart>> CreateCartAsync(Domain.Entities.Cart cart, CancellationToken cancellationToken = default);
    Task<Result> UpdateCartAsync(Domain.Entities.Cart cart, CancellationToken cancellationToken = default);
    Task<Result> DeleteCartAsync(Guid id, CancellationToken cancellationToken = default);
}