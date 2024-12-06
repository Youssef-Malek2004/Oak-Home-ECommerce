using System.Linq.Expressions;
using Abstractions.ResultsPattern;
using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;
using Orders.Domain.Errors;
using Orders.Domain.Repositories;

namespace Orders.Infrastructure.Persistence.Repositories;

public class OrdersRepository(OrdersDbContext context) : IOrdersRepository
{
    public async Task<Result<IEnumerable<Order>>> GetOrdersAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var orders = await context.Orders.ToListAsync(cancellationToken);
            return Result<IEnumerable<Order>>.Success(orders);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Order>>.Failure(OrderErrors.OrderQueryFailed(ex.Message));
        }
    }

    public async Task<Result<Order?>> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId, cancellationToken);

            if (order == null)
            {
                return Result<Order?>.Failure(OrderErrors.OrderNotFound(orderId));
            }

            return Result<Order?>.Success(order);
        }
        catch (Exception ex)
        {
            return Result<Order?>.Failure(OrderErrors.OrderQueryFailed(ex.Message));
        }
    }

    public async Task<Result<IEnumerable<OrderItem>>> GetOrderItemsByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        try
        {
            var orderItems = await context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync(cancellationToken);

            if (!orderItems.Any())
            {
                return Result<IEnumerable<OrderItem>>.Failure(OrderErrors.OrderItemsNotFound(orderId));
            }

            return Result<IEnumerable<OrderItem>>.Success(orderItems);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<OrderItem>>.Failure(OrderErrors.OrderItemQueryFailed(ex.Message));
        }
    }

    public async Task<Result> AddOrderAsync(Order order)
    {
        try
        {
            await context.Orders.AddAsync(order);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(OrderErrors.OrderAddFailed(ex.Message));
        }
    }

    public async Task<Result> RemoveOrderAsync(Order order)
    {
        try
        {
            context.Orders.Remove(order);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(OrderErrors.OrderRemoveFailed(ex.Message));
        }
    }

    public async Task<Result> UpdateOrderAsync(Order order)
    {
        try
        {
            context.Orders.Update(order);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(OrderErrors.OrderUpdateFailed(ex.Message));
        }
    }

    public async Task<Result<IEnumerable<Order>>> GetOrdersByConditionAsync(
        Expression<Func<Order, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var orders = await context.Orders
                .Where(predicate)
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<Order>>.Success(orders);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Order>>.Failure(OrderErrors.OrderQueryFailed(ex.Message));
        }
    }
}
