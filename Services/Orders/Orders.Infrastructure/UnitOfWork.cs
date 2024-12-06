using Abstractions.ResultsPattern;
using Orders.Domain;
using Orders.Domain.Repositories;
using Orders.Infrastructure.Persistence;
using Orders.Infrastructure.Persistence.Repositories;

namespace Orders.Infrastructure;

public class UnitOfWork(OrdersDbContext context) : IUnitOfWork
{
    private IOrdersRepository? _ordersRepository;

    public IOrdersRepository OrdersRepository => _ordersRepository ??= new OrdersRepository(context);

    public async Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
