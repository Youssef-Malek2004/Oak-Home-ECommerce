using System.Linq.Expressions;
using Abstractions.ResultsPattern;
using Orders.Domain.Entities;

namespace Orders.Domain.Repositories;

public interface IOrdersRepository
{
    Task<Result<IEnumerable<Order>>> GetOrdersAsync(CancellationToken cancellationToken = default);
    Task<Result<Order?>> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<OrderItem>>> GetOrderItemsByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);

    Task<Result> AddOrderAsync(Order order);

    Task<Result> RemoveOrderAsync(Order order);

    Task<Result> UpdateOrderAsync(Order order);

    Task<Result<IEnumerable<Order>>> GetOrdersByConditionAsync(
        Expression<Func<Order, bool>> predicate,
        CancellationToken cancellationToken = default);
}